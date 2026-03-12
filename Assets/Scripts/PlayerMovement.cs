using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    [Header("Footsteps")]
    public AudioSource footstepSource;

    public AudioClip leftFootstep;
    public AudioClip rightFootstep;

    [Header("Wood Footstep")]
    public AudioClip woodFootstep;

    public float walkStepDelay = 0.5f;

    private float stepTimer;
    private bool isLeftStep = true;

    private string currentSurface = "Ground";

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

    void Update()
    {
        Move();
        Look();
        ApplyGravity();
        HandleFootsteps();
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

    void HandleFootsteps()
    {
        bool isMoving = moveInput.magnitude > 0.1f;
        bool grounded = controller.isGrounded;

        if (isMoving && grounded)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                AudioClip clipToPlay = null;

                if (currentSurface == "Wood")
                {
                    clipToPlay = woodFootstep;
                }
                else
                {
                    clipToPlay = isLeftStep ? leftFootstep : rightFootstep;
                }

                if (clipToPlay != null)
                    footstepSource.PlayOneShot(clipToPlay);

                isLeftStep = !isLeftStep;
                stepTimer = walkStepDelay;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        currentSurface = hit.gameObject.tag;

        if (hit.gameObject.CompareTag("Mushroom"))
        {
            if (hit.normal.y > 0.5f)
            {
                velocity.y = Mathf.Sqrt((jumpForce * 2) * -2f * gravity);
            }
        }

        if (hit.gameObject.CompareTag("Enemy"))
        {
            SceneManager.LoadSceneAsync(2);
        }

        if (hit.gameObject.CompareTag("Hat"))
        {
            SceneManager.LoadSceneAsync(3);
        }
    }
}