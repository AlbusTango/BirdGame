using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NewGravityController_Touch : MonoBehaviour
{
    public enum GravityDirection
    {
        Left,
        Right
    }

    [SerializeField] private bool debugAlwaysGrounded;

    [Header("Gravity Settings")]
    public GravityDirection gravityDirection = GravityDirection.Right;
    public float gravityStrength = 9.81f;           // sideways gravity
    public float gravityIncreaseRate = 0.2f;
    public float maxGravityStrength = 22.81f;

    public float downwardGravityStrength = 12f;     // current downward gravity
    public float downwardGravityIncreaseRate = 0.2f;
    public float maxDownwardGravityStrength = 22f;

    private bool forceFall;
    private float forceFallTime = 0.1f;
    private float forcedFallTimer;

    [Header("Jump Settings")]
    public float jumpForce = 7f;
    public float maxJumpForce = 20f;
    public float jumpIncreaseRate = 0.2f;
    private AudioSource jumpAudio;
    private AudioSource flipAudio;

    [Header("Touch Controls")]
    private Vector2 touchStartPos;
    public float swipeThreshold = 50f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckDistance = 0.6f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    private Transform visuals;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private LogicManager logic;
    private PauseManager pause;
    private bool controlsEnabled = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        visuals = transform.Find("Visuals");
        if (visuals == null) visuals = transform;

        spriteRenderer = visuals.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            Debug.LogError("SpriteRenderer not found on Visuals or Player! Please add it.");

        animator = GetComponentInChildren<Animator>();

        AudioSource[] sources = GetComponents<AudioSource>();
        if (sources.Length >= 2)
        {
            jumpAudio = sources[0];
            flipAudio = sources[1];
        }

        logic = FindFirstObjectByType<LogicManager>();
        pause = FindAnyObjectByType<PauseManager>();
    }

    void Update()
    {
        if (!controlsEnabled) return;

        HandleTouchInput();

        // gradually increase jump & gravity if needed
        jumpForce = Mathf.Min(jumpForce + jumpIncreaseRate * Time.deltaTime, maxJumpForce);
        gravityStrength = Mathf.Min(gravityStrength + gravityIncreaseRate * Time.deltaTime, maxGravityStrength);
        downwardGravityStrength = Mathf.Min(
            downwardGravityStrength + downwardGravityIncreaseRate * Time.deltaTime,
            maxDownwardGravityStrength
        );

        // manage forced fall timing
        if (forceFall)
        {
            forcedFallTimer -= Time.deltaTime;
            if (forcedFallTimer <= 0f)
                forceFall = false;
        }
    }

    void FixedUpdate()
    {
        CheckGrounded();
        ApplyCustomGravity();
        UpdateAnimator();
    }

    void ApplyCustomGravity()
    {
        Vector2 sideGravity = (gravityDirection == GravityDirection.Right ? Vector2.right : Vector2.left);
        Vector2 totalGravity = sideGravity * gravityStrength;

        // Only apply downward gravity if not grounded
        if (!isGrounded)
            totalGravity += Vector2.down * downwardGravityStrength;

        rb.AddForce(totalGravity, ForceMode2D.Force);
    }

    // Fixed jump: horizontal opposite force + upward lift
    void Jump()
    {
        if (!isGrounded || !controlsEnabled) return;

        isGrounded = false;

        Vector2 horizontalJumpDir = (gravityDirection == GravityDirection.Right ? Vector2.left : Vector2.right);
        Vector2 jumpVector = horizontalJumpDir * jumpForce + Vector2.up * (jumpForce * 0.6f);

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(jumpVector, ForceMode2D.Impulse);

        if (jumpAudio != null)
            jumpAudio.Play();
    }

    void CheckGrounded()
    {
        if (debugAlwaysGrounded)
        {
            isGrounded = true;
            return;
        }

        if (groundCheck == null)
        {
            isGrounded = false;
            Debug.LogError("GroundCheck is NULL!");
            return;
        }

        Vector2 pos = groundCheck.position;
        RaycastHit2D hitRight = Physics2D.Raycast(pos, Vector2.right, groundCheckDistance, groundLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(pos, Vector2.left, groundCheckDistance, groundLayer);

        isGrounded = hitRight.collider != null || hitLeft.collider != null;
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                touchStartPos = touch.position;
            else if (touch.phase == TouchPhase.Ended)
                TouchOrSwipe(touch.position - touchStartPos);

            return;
        }

        if (Input.GetMouseButtonDown(0))
            touchStartPos = Input.mousePosition;
        else if (Input.GetMouseButtonUp(0))
            TouchOrSwipe((Vector2)Input.mousePosition - touchStartPos);
    }

    void TouchOrSwipe(Vector2 delta)
    {
        if (!controlsEnabled) return;

        if (Mathf.Abs(delta.x) > swipeThreshold)
        {
            SetGravityDirection(delta.x > 0 ? GravityDirection.Right : GravityDirection.Left);
            return;
        }

        if (isGrounded)
            Jump();
    }

    void SetGravityDirection(GravityDirection dir)
    {
        if (!controlsEnabled || gravityDirection == dir) return;

        gravityDirection = dir;

        if (spriteRenderer != null)
            spriteRenderer.flipY = (dir == GravityDirection.Left);

        if (flipAudio != null)
            flipAudio.Play();

        // Flip + small jump upward
        float wallJumpForce = jumpForce * 0.15f; // tweak this if needed
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // reset vertical velocity only
        rb.AddForce(Vector2.up * wallJumpForce, ForceMode2D.Impulse);

        ForceFall();
    }

    void UpdateAnimator()
    {
        if (animator == null) return;

        animator.SetBool("isGrounded", isGrounded);

        if (forceFall)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
            return;
        }

        if (isGrounded)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
            return;
        }

        bool falling = gravityDirection == GravityDirection.Right
            ? rb.linearVelocity.x > 0.05f
            : rb.linearVelocity.x < -0.05f;

        animator.SetBool("isFalling", falling);
        animator.SetBool("isJumping", !falling);
    }

    void ForceFall()
    {
        isGrounded = false;
        forceFall = true;
        forcedFallTimer = forceFallTime;

        if (animator == null) return;

        animator.SetBool("isGrounded", false);
        animator.SetBool("isJumping", false);
        animator.SetBool("isFalling", true);
    }

    public void DisableControls() => controlsEnabled = false;
    public void EnableControls() => controlsEnabled = true;
}