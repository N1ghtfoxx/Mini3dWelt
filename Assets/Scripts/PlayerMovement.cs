using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 movementInput;
    private Transform camTransform;
    [SerializeField] private float movementSpeed;

    // This method is called when the script instance is being loaded
    void Start()
    {
        camTransform = Camera.main.transform;
    }

    // This method is called when the player provides input for movement
    public void Movement(CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
    }

    // This method is called at a fixed interval, typically used for physics calculations
    private void FixedUpdate()
    {
       var movementDirection = movementInput.x * camTransform.right + movementInput.y * Vector3.ProjectOnPlane(camTransform.forward, Vector3.up).normalized;
        transform.Translate(movementSpeed * Time.deltaTime * movementDirection);
    }
}
