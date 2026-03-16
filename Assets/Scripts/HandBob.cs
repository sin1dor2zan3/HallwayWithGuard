using UnityEngine;

public class HandBob : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float bobSpeed = 5f;
    [SerializeField] float bobAmount = 0.006f;
    [SerializeField] float moveThreshold = 0.0005f;
    [SerializeField] float smoothSpeed = 8f;

    [Header("Jump / Landing")]
    [SerializeField] float jumpDipAmount = 0.02f;
    [SerializeField] float landBounceAmount = 0.015f;
    [SerializeField] float jumpSmooth = 10f;

    [Header("Mushroom Bounce")]
    [SerializeField] float mushroomBounceAmount = 0.05f;

    Vector3 startPos;
    Vector3 lastPlayerPos;
    float timer;

    CharacterController controller;
    bool wasGrounded;
    float jumpOffsetY;

    void Start()
    {
        startPos = transform.localPosition;

        if (player != null)
        {
            lastPlayerPos = player.position;
            controller = player.GetComponent<CharacterController>();

            if (controller != null)
                wasGrounded = controller.isGrounded;
        }
    }

    void Update()
    {
        if (player == null)
            return;

        Vector3 currentPlayerPos = player.position;
        Vector3 movement = currentPlayerPos - lastPlayerPos;
        movement.y = 0f;

        bool isMoving = movement.sqrMagnitude > moveThreshold;

        Vector3 targetPos = startPos;

        // walking bob
        if (isMoving)
        {
            timer += Time.deltaTime * bobSpeed;

            float bobX = Mathf.Sin(timer) * bobAmount;
            float bobY = Mathf.Abs(Mathf.Cos(timer)) * bobAmount;

            targetPos += new Vector3(bobX, bobY, 0f);
        }
        else
        {
            timer = 0f;
        }

        // jump / landing motion
        float targetJumpOffset = 0f;

        if (controller != null)
        {
            bool isGrounded = controller.isGrounded;

            if (wasGrounded && !isGrounded)
            {
                jumpOffsetY = -jumpDipAmount;
            }

            if (!wasGrounded && isGrounded)
            {
                jumpOffsetY = landBounceAmount;
            }

            wasGrounded = isGrounded;
        }

        jumpOffsetY = Mathf.Lerp(jumpOffsetY, targetJumpOffset, Time.deltaTime * jumpSmooth);
        targetPos += new Vector3(0f, jumpOffsetY, 0f);

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPos,
            Time.deltaTime * smoothSpeed
        );

        lastPlayerPos = currentPlayerPos;
    }

    public void TriggerMushroomBounce()
    {
        jumpOffsetY = -mushroomBounceAmount;
    }
}