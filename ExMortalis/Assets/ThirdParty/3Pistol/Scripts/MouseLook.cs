using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // Floats
    public float sensitivity;

    // Booleans
    private bool isCursorLocked = true;

    // Transforms
    private Transform targetCharacter;
    private Transform targetCamera;



    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        targetCamera = transform;
        targetCharacter = gameObject.transform.parent.transform;

    }


    public void Update()
    {
        // Get current input
        float x = Input.GetAxis("Mouse X") * sensitivity;
        float y = Input.GetAxis("Mouse Y") * sensitivity;

        // Update the character and camera rotation
        targetCharacter.transform.Rotate(Vector3.up, x * Time.deltaTime);
        targetCamera.Rotate(Vector3.right, -y * Time.deltaTime);

        // Update the cursor lock
        CheckForInput();
    }



    // This function manages to toggle the cursor
    public void ToggleCursor()
    {
        // Toggle the cursor boolean
        isCursorLocked = !isCursorLocked;

        // Should the cursor locked?
        if (!isCursorLocked)
        {
            // If yes, then lock it
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // If no, then unlock it
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }



    private void CheckForInput()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ToggleCursor();
        }
    }
}
