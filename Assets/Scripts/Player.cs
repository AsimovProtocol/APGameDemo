using Classes;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Expose player controls
    [SerializeField] private float jumpForce = 300f;
    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float lookSpeedMouse = 0.2f;
    [SerializeField] private float lookSpeedGamepad = 5f;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask playerMeshMask = 0;
    [SerializeField] private Transform playerGroundCollider;
    private CameraLooking cameraLooking;
    private Transform ct;
    private Vector2 gamepadLook;

    // Variables
    private bool inputJump;
    private Vector2 inputMove;
    private bool onGround;
    private PlayerInput playerInput;

    // Create references
    private Rigidbody rb;
    private float totalPitch;
    private Transform tr;

    // Start is called before the first frame update
    private void Start()
    {
        //hide cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        // initialize references
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        ct = playerCamera.transform;
        playerInput = GetComponent<PlayerInput>();
        cameraLooking = playerCamera.GetComponent<CameraLooking>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void FixedUpdate()
    {
        onGround = Physics.CheckSphere(playerGroundCollider.position, 0.05f, playerMeshMask);
        HandleMovement();
    }

    /**
     * Rotate camera and limit the player look angle
     */
    private void RotateCameraPitch(float delta)
    {
        var pitch = totalPitch;
        totalPitch = Mathf.Clamp(totalPitch + delta, -90, 90);
        ct.Rotate(Vector3.left, totalPitch - pitch);
    }

    /**
     * Called during FixedUpdate, handles player movement
     */
    private void HandleMovement()
    {
        if (inputJump)
        {
            if (onGround) rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            inputJump = false;
        }

        rb.AddRelativeForce((onGround ? 1 : 0.5f) * moveSpeed * new Vector3(inputMove.x, 0, inputMove.y), ForceMode.VelocityChange);

        if (playerInput.currentControlScheme == "Gamepad")
        {
            tr.Rotate(Vector3.up, gamepadLook.x * lookSpeedGamepad);
            RotateCameraPitch(gamepadLook.y * lookSpeedGamepad);
        }
    }

    private void OnMove(InputValue value)
    {
        inputMove = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        inputJump = true;
    }

    private void OnLook(InputValue value)
    {
        var look = value.Get<Vector2>();
        switch (playerInput.currentControlScheme)
        {
            case "Keyboard&Mouse":
                tr.Rotate(Vector3.up, look.x * lookSpeedMouse);
                RotateCameraPitch(look.y * lookSpeedMouse);
                break;
            case "Gamepad":
                gamepadLook = look;
                break;
        }
    }

    private void OnInteract(InputValue value)
    {
        foreach (var i in cameraLooking.looking.GetComponents<IInteractable>()) i.Interact();
    }
}