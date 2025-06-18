using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerControllerUnterricht : MonoBehaviour
{

    private PlayerInput playerInput;
    private Vector2 movementInput;
    private Transform cameraTransform;
    [SerializeField] private float actualMovementSpeed = 5f;
    [SerializeField] private float jumpStrength;
    private Rigidbody rb;
    private bool isGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;

        playerInput.actions["Jump"].performed += OnJump;

        cameraTransform = Camera.main.transform;

        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
    }

    public void OnJump(CallbackContext ctx)
    {
        if (!isGrounded) return; // Only allow jumping if the player is grounded
        rb.AddForce(Vector3.up * jumpStrength); // Add an upward force for jumping
        Debug.Log("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        var movementDirection = cameraTransform.right * movementInput.x + cameraTransform.forward * movementInput.y;
        movementDirection = Vector3.ProjectOnPlane(movementDirection, Vector3.up).normalized; // Project movement direction onto the horizontal plane

        transform.Translate(movementDirection * actualMovementSpeed * Time.deltaTime);
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true; // Set isGrounded to true when colliding with the ground
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false; // Set isGrounded to false when no longer colliding with the ground
    }
}
