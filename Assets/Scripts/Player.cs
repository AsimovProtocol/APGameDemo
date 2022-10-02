using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Expose player controls
    [SerializeField] private float jumpForce = 300f;
    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float lookSpeedMouse = 0.5f;
    [SerializeField] private float lookSpeedGamepad = 5f;
    [SerializeField] private Camera playerCamera = null;
    [SerializeField] private LayerMask playerMeshMask = 0;
    [SerializeField] private Transform playerGroundCollider = null;

    // Create references
    private Rigidbody rb;
    private Transform tr;
    private Transform ct;
    private PlayerInput playerInput;

    // Variables
    private bool inputJump;
    private Vector2 inputMove;
    private Vector2 gamepadLook;
    private float totalPitch;
    private bool onGround;

    // Start is called before the first frame update
    private void Start()
    {
        // initialize references
        this.rb = GetComponent<Rigidbody>();
        this.tr = GetComponent<Transform>();
        this.ct = playerCamera.transform;
        this.playerInput = GetComponent<PlayerInput>();
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
        float pitch = totalPitch;
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
        this.inputMove = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        this.inputJump = true;
    }

    private void OnLook(InputValue value)
    {
        Vector2 look = value.Get<Vector2>();
        switch (playerInput.currentControlScheme)
        {
            case "Keyboard&Mouse":
                tr.Rotate(Vector3.up, look.x * lookSpeedMouse);
                RotateCameraPitch(look.y * lookSpeedMouse);
                break;
            case "Gamepad":
                this.gamepadLook = look;
                break;
        }
    }
}