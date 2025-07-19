using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

//summary:
// This script handles player movement based on input from the Input System package.
// It allows the player to move in the direction of the camera's orientation.
public class PlayerMovement : MonoBehaviour
{
    private Vector2 movementInput;
    private Transform camTransform;
    [SerializeField] private float movementSpeed;

    // summary:
    // This method is called when the script instance is being loaded
    // It initializes the camera transform to the main camera's transform.
    void Start()
    {
        camTransform = Camera.main.transform;
    }

    // summary:
    // This method is called when the player provides movement input
    // It reads the input value and stores it in the movementInput variable.
    public void Movement(CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
    }

    // summary:
    // This method is called at a fixed interval to update the player's position
    // It calculates the movement direction based on the camera's orientation and applies the movement speed.
    private void FixedUpdate()
    {
       var movementDirection = movementInput.x * camTransform.right + movementInput.y * Vector3.ProjectOnPlane(camTransform.forward, Vector3.up).normalized;
        transform.Translate(movementSpeed * Time.deltaTime * movementDirection);
    }
}
