using NL.ExMORTALIS.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NL.ExMORTALIS
{
    public class FirstPersonController : MonoBehaviour
    {
        private const float _threshold = 0.01f;
        private Entity m_PlayerEntity;
        private CapsuleCollider m_CapsuleCollider;
        private CharacterController m_CharacterController;
        private Rigidbody m_Rigidbody;
        private PlayerInput m_PlayerInput;

        [SerializeField] private Camera m_Camera;
        [SerializeField][Range(0.1f, 1f)] private float m_HorizontalSensitivity;
        [SerializeField][Range(0.1f, 1f)] private float m_VerticalSensitivity;
        [SerializeField][Range(1f, 10f)] private float m_MoveSpeed;
        [SerializeField][Range(1f, 2f)] private float m_JumpForce;
        [SerializeField][Range(3f, 10f)] private float m_CrouchTransitionSpeed;
        [SerializeField] private float m_Gravity = 9.81f;
        [SerializeField] private float m_GroundCheckDistance = 0.2f;
        [SerializeField] private LayerMask m_GroundMask;
        [SerializeField] private float m_GroundedOffset = -0.14f;
        [SerializeField] private float m_GroundedRadius = 0.5f;

        [SerializeField] private float m_FallTimeout;
        [SerializeField] private float m_JumpTimeout;
        [SerializeField] private float m_TerminalVelocity;
        private float m_FallTimeoutDelta;
        private float m_JumpTimeoutDelta;

        private float m_StandHeight;
        private float m_CurrentHeight;
        private float m_CrouchHeight => m_StandHeight - (m_StandHeight * 0.40f);
        private float m_CameraCrouchHeight => m_InitialCamPosition.y - (m_InitialCamPosition.y * .40f);
        private float m_CrouchSpeed => m_MoveSpeed - (m_MoveSpeed * 0.40f);

        private float m_VerticalVelocity = 0f;
        private float m_RotationX;
        private float m_RotationY;
        private bool m_IsJumping = false;
        private bool m_IsGrounded = false;
        private bool m_WasAirborne = false;
        private bool m_IsCrouching = false;
        private Vector3 m_InitialCamPosition;

        [SerializeField] private AnimationCurve m_JumpCurve;
        [SerializeField] private AnimationCurve m_CrouchCurve;
        [SerializeField] private AnimationCurve m_StandCurve;
        [SerializeField] private AnimationCurve m_SpeedCurve;

        private PlayerControls m_PlayerControls;
        private Vector2 MouseLook => m_PlayerControls.Gameplay.Look.ReadValue<Vector2>();
        private Vector2 Move => m_PlayerControls.Gameplay.Move.ReadValue<Vector2>();
        private bool IsCurrentDeviceKBM
        {
            get
            {
                return m_PlayerInput.currentControlScheme == "KeyboardMouse";
            }
        }

        private void Awake()
        {
            TryGetComponent<Entity>(out m_PlayerEntity);
            TryGetComponent<CapsuleCollider>(out m_CapsuleCollider);
            TryGetComponent<CharacterController>(out m_CharacterController);
            TryGetComponent<Rigidbody>(out m_Rigidbody);
            TryGetComponent<PlayerInput>(out m_PlayerInput);
            m_PlayerControls = new PlayerControls();

            m_PlayerControls.Gameplay.Jump.performed += _ => Jump();
            m_PlayerControls.Gameplay.Crouch.performed += _ => StartCrouch();
            m_PlayerControls.Gameplay.Crouch.canceled += _ => StopCrouch();
        }

        private void Start()
        {
            m_PlayerControls.Enable();
            m_StandHeight = m_CapsuleCollider.height;
            m_CurrentHeight = m_StandHeight;
            m_InitialCamPosition = m_Camera.transform.localPosition;
        }

        private void Update()
        {
            Vector3 moveDirection = new Vector3(Move.x, 0, Move.y).normalized;
            moveDirection = transform.TransformDirection(moveDirection);

            JumpAndGravity();
            GroundedCheck();


            if (m_IsCrouching)
            {
                float heightDelta = m_CurrentHeight / m_CrouchHeight;
                m_CurrentHeight = m_CrouchHeight * m_CrouchCurve.Evaluate(heightDelta);

                float cameraHeightDelta = m_CurrentHeight / m_CrouchHeight;
                m_CurrentHeight = m_CrouchHeight * m_CrouchCurve.Evaluate(cameraHeightDelta);

                m_Camera.transform.localPosition = new Vector3(0, m_CameraCrouchHeight, 0);
            }
            else
            {
                float heightDelta = m_CurrentHeight / m_StandHeight;
                m_CurrentHeight = m_StandHeight * m_StandCurve.Evaluate(heightDelta);

                m_Camera.transform.localPosition = Vector3.Lerp(m_Camera.transform.localPosition, m_InitialCamPosition, m_CrouchTransitionSpeed * Time.deltaTime);

                if (Mathf.Approximately(m_CurrentHeight, m_StandHeight))
                {
                    m_CurrentHeight = m_StandHeight;
                }
            }

            m_CapsuleCollider.height = Mathf.Lerp(m_CapsuleCollider.height, m_CurrentHeight, m_CrouchTransitionSpeed * Time.deltaTime);
            m_CapsuleCollider.height = m_CapsuleCollider.height;

            moveDirection.y = Mathf.Lerp(moveDirection.y, m_VerticalVelocity, 10 * Time.deltaTime);

            m_CharacterController.Move(moveDirection * m_MoveSpeed * Time.deltaTime);
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void JumpAndGravity()
        {
            if (m_IsGrounded)
            {
                m_FallTimeoutDelta = m_FallTimeout;

                //if (m_VerticalVelocity < 0f)
                //{
                //    m_VerticalVelocity = -2;
                //}

                if (m_IsJumping && m_JumpTimeoutDelta <= 0.0f)
                {
                    m_VerticalVelocity = Mathf.Sqrt(m_JumpForce * -2f * -m_Gravity);
                }

                if (m_JumpTimeoutDelta >= 0.0f)
                {
                    m_JumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                m_JumpTimeoutDelta = m_JumpTimeout;

                if (m_FallTimeoutDelta >= 0.0f)
                {
                    m_FallTimeoutDelta -= Time.deltaTime;
                }

                if (m_VerticalVelocity < m_TerminalVelocity)
                {
                    m_VerticalVelocity -= m_Gravity * Time.deltaTime;
                }
            }
        }

        private void Jump()
        {
            if (m_IsGrounded)
            {
                m_IsJumping = true;
                m_JumpTimeoutDelta = 0;
            }
        }

        private void StartCrouch()
        {
            m_IsCrouching = true;
        }

        private void StopCrouch()
        {
            m_IsCrouching = false;
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (m_IsGrounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y - m_GroundedOffset, transform.position.z), m_GroundedRadius);
        }

        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - m_GroundedOffset, transform.position.z);
            m_IsGrounded = Physics.CheckSphere(spherePosition, m_GroundedRadius, m_GroundMask, QueryTriggerInteraction.Ignore);
        }

        private void CameraRotation()
        {
            m_RotationX += MouseLook.y * m_VerticalSensitivity;
            m_RotationX = Mathf.Clamp(m_RotationX, -90f, 90f);

            transform.Rotate(0, MouseLook.x * m_HorizontalSensitivity, 0);
            Camera.main.transform.localRotation = Quaternion.Euler(m_RotationX, 0, 0);
        }
    }
}
