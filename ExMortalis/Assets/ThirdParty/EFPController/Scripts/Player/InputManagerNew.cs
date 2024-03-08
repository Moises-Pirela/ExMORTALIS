using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static EFPController.InputManager;
using Transendence.Core;
using Transendence.Core.Postprocess;
using UnityEngine.PlayerLoop;

namespace EFPController
{

    [DefaultExecutionOrder(-997)]
    public class InputManagerNew : InputManager
    {

        public enum Axis
        {
            Movement,
            MouseLook,
            ControllerLook,
        }

        [Serializable]
        public struct InputAxisData
        {
            public Axis axis;
            public InputActionReference inputAction;
        }

        [Serializable]
        public struct InputActionData
        {
            public Action action;
            public InputActionReference inputAction;
        }

        [SerializeField]
        private List<InputAxisData> axis = new List<InputAxisData>();
        [SerializeField]
        private List<InputActionData> actions = new List<InputActionData>();

        private InputActions m_InputActions;

        private void Awake()
        {
            instance = this;

            m_InputActions = new InputActions();
            
            // m_InputActions.Gameplay._1.performed += _ => FireWeapon(0);
            // m_InputActions.Gameplay._2.performed += _ => FireWeapon(1);
            // m_InputActions.Gameplay._3.performed += _ => FireWeapon(2);
            // m_InputActions.Gameplay._4.performed += _ => FireWeapon(3);
            // m_InputActions.Gameplay._5.performed += _ => FireWeapon(0);
            // m_InputActions.Gameplay._6.performed += _ => FireWeapon(1);
            // m_InputActions.Gameplay._7.performed += _ => FireWeapon(2);
            // m_InputActions.Gameplay._8.performed += _ => FireWeapon(3);
            // m_InputActions.Gameplay._9.performed += _ => FireWeapon(3);
            // m_InputActions.Gameplay._0.performed += _ => FireWeapon(3);
            m_InputActions.Gameplay.PrimaryFire.performed += _ => UseEquippedItem(0); 
            m_InputActions.Gameplay.Cycle.performed += (InputAction.CallbackContext c) => CycleWeapons((int)c.ReadValue<float>());
        }

        void Start()
        {
            InputSystem.onEvent += (eventPtr, device) =>
            {
                if (device != null) isGamepad = device is Gamepad;
            };

            foreach (InputAxisData axisData in axis)
            {
                if (axisData.inputAction != null) axisData.inputAction.action.Enable();
            }
            
            foreach (InputActionData actionData in actions)
            {
                if (actionData.inputAction != null) actionData.inputAction.action.Enable();
            }

            m_InputActions.Enable();
        }

        protected override Vector2 GetMouseLookInput()
        {
            if (axis.Exists(x => x.axis == Axis.MouseLook && x.inputAction != null))
            {
                InputAxisData iad = axis.First(x => x.axis == Axis.MouseLook);
                return iad.inputAction.action.ReadValue<Vector2>();
            }
            return Vector2.zero;
        }

        protected override Vector2 GetControllerLookInput()
        {
            if (axis.Exists(x => x.axis == Axis.ControllerLook && x.inputAction != null))
            {
                InputAxisData iad = axis.First(x => x.axis == Axis.ControllerLook);
                return iad.inputAction.action.ReadValue<Vector2>();
            }
            return Vector2.zero;
        }

        public override Vector2 GetMovementInput()
        {
            if (axis.Exists(x => x.axis == Axis.Movement && x.inputAction != null))
            {
                InputAxisData iad = axis.First(x => x.axis == Axis.Movement);
                return iad.inputAction.action.ReadValue<Vector2>();
            }
            return Vector2.zero;
        }

        // IsPressed
        public override bool GetActionKey(Action action)
        {
            if (actions.Exists(x => x.action == action && x.inputAction != null))
            {
                InputActionData iad = actions.First(x => x.action == action);
                return iad.inputAction.action.IsPressed();
            }
            return false;
        }

        // WasReleasedThisFrame
        public override bool GetActionKeyUp(Action action)
        {
            if (actions.Exists(x => x.action == action && x.inputAction != null))
            {
                InputActionData iad = actions.First(x => x.action == action);
                return iad.inputAction.action.WasReleasedThisFrame();
            }
            return false;
        }

        // WasPerformedThisFrame
        public override bool GetActionKeyDown(Action action)
        {
            if (actions.Exists(x => x.action == action && x.inputAction != null))
            {
                InputActionData iad = actions.First(x => x.action == action);
                return iad.inputAction.action.WasPerformedThisFrame();
            }
            return false;
        }

        public void UseEquippedItem(int weaponIndex)
        {
            UseWeaponPostprocessEvent use = new UseWeaponPostprocessEvent();

            TryGetComponent<Entity>(out Entity playerEntity);

            use.WeaponIndex = 0;
            use.WeaponHolderEntityId = playerEntity.Id;
            use.WeaponUseType = WeaponUseType.Shoot;

            World.Instance.AddPostProcessEvent(use);
        }

        public void CycleWeapons(int cycleAmount)
        {
            CycleWeaponPostProcessEvent cycle = new CycleWeaponPostProcessEvent();

            TryGetComponent<Entity>(out Entity playerEntity);

            cycle.CycleAmount = cycleAmount;
            cycle.EntityCycleId = playerEntity.Id;

            World.Instance.AddPostProcessEvent(cycle);
        }

    }

}