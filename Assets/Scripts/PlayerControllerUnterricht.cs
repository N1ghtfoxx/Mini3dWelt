using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerControllerUnterricht : MonoBehaviour
{

    private PlayerInput playerInput;
    private Vector2 movementInput;
    private Transform cameraTransform;
    [SerializeField] private float actualMovementSpeed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;

        cameraTransform = Camera.main.transform;
        //cameraTransform.forward = transform.forward; // Ensure the camera's forward direction is aligned with the player's forward direction
    }

    public void OnMove(CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        var movementDirection = cameraTransform.right * movementInput.x + cameraTransform.forward * movementInput.y;
        movementDirection = Vector3.ProjectOnPlane(movementDirection, Vector3.up).normalized; // Project movement direction onto the horizontal plane

        transform.Translate(movementDirection * actualMovementSpeed * Time.deltaTime);
    }
}
