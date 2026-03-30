using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityController : MonoBehaviour
{
    public enum GravityDirection
    {
        Left,
        Right
    }

    [Header("Gravity Settings")]
    public GravityDirection gravityDirection = GravityDirection.Right;
    public float gravityStrength = 9.81f;
    public float gravityIncreaseRate = 0.2f;
    public float maxGravityStrength = 22.81f;

    [Header("Jump Settings")]
    public float jumpForce = 7f;
    public KeyCode jumpKey = KeyCode.Space;
    public float maxJumpForce = 20f;
    public float jumpIncreaseRate = 0.2f;
    private AudioSource jumpAudio;


    [Header("Flip Control")]
    public KeyCode flipKey = KeyCode.Z; // Toggle gravity between left/right
    private AudioSource flipAudio;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckDistance = 0.3f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Disable built-in gravity

        AudioSource[] sources = GetComponents<AudioSource>();
        if (sources.Length > 0) jumpAudio = sources[0];
        if (sources.Length > 1) flipAudio = sources[1];
    }

    void Update()
    {
        HandleFlipInput();
        CheckGrounded();

        jumpForce = Mathf.Min(jumpForce + jumpIncreaseRate * Time.deltaTime, maxJumpForce);
        gravityStrength = Mathf.Min(gravityStrength + gravityIncreaseRate * Time.deltaTime, maxGravityStrength);

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        ApplyCustomGravity();
    }

    void HandleFlipInput()
    {
        // Flip gravity horizontally when pressing flipkey
        if (Input.GetKeyDown(flipKey))
        {
            gravityDirection = gravityDirection == GravityDirection.Right
                ? GravityDirection.Left
                : GravityDirection.Right;

            // Optional: flip sprite visually
            Vector3 scale = transform.localScale;
            scale.y *= -1;
            transform.localScale = scale;

            if (flipAudio != null)
            {
                flipAudio.Play();
            }
        }
    }

    void ApplyCustomGravity()
    {
        Vector2 gravityVector = gravityDirection == GravityDirection.Right
            ? Vector2.right
            : Vector2.left;

        rb.AddForce(gravityVector * gravityStrength);
    }

    void Jump()
    {
        // Jump opposite the gravity direction
        Vector2 jumpDir = gravityDirection == GravityDirection.Right
            ? Vector2.left
            : Vector2.right;

        rb.linearVelocity = Vector2.zero; // reset current movement
        rb.AddForce(jumpDir * jumpForce, ForceMode2D.Impulse);

        if (jumpAudio != null)
        {
            jumpAudio.Play();
        }
    }

    void CheckGrounded()
    {
        if (groundCheck == null)
        {
            isGrounded = true;
            return;
        }

        Vector2 checkDir = gravityDirection == GravityDirection.Right
            ? Vector2.right
            : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, checkDir, groundCheckDistance, groundLayer);
        isGrounded = hit.collider != null;
    }
}