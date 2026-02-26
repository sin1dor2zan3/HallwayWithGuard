using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 1.5f;
    public float gravity = -9.8f;

    [Header("Look")]
    public float mouseSensitivity = 0.15f;
    public float stickSensitivity = 180f;
    public Transform cameraPivot;

    CharacterController controller;
    PlayerControls controls;

    Vector2 moveInput;
    Vector2 lookInput;
    Vector3 velocity;
    float xRotation;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += _ => moveInput = Vector2.zero;

        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += _ => lookInput = Vector2.zero;

        controls.Player.Jump.performed += _ => Jump();
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Move();
        Look();
        ApplyGravity();
        CursorToggle();
    }

    void Move()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    void Look()
    {
        if (lookInput.magnitude < 0.05f)
            return;

        bool stick = Mathf.Abs(lookInput.x) <= 1 && Mathf.Abs(lookInput.y) <= 1;
        float sens = stick ? stickSensitivity : mouseSensitivity;

        float x = lookInput.x * sens;
        float y = lookInput.y * sens;

        if (stick)
        {
            x *= Time.deltaTime;
            y *= Time.deltaTime;
        }

        xRotation -= y;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * x);
    }

    void Jump()
    {
        if (controller.isGrounded)
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
    }

    void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void CursorToggle()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame &&
            Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}