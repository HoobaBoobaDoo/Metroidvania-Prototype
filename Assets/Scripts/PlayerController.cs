using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerControls controls;
    private Vector2 moveInput;

    private Rigidbody2D rb;

    
    [Header("Player Movement")]

    public float moveSpeed = 8f;
    public float jumpStrength;

    private bool jumpPossible;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Animator")]
    public Animator animator;

    private bool isGrounded = false;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Jump.performed += ctx => JumpPressed();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckGround();

        if (isGrounded && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            animator.SetBool("isJumping", false);
            jumpPossible = true;
        }
        else
        {
            jumpPossible = false;
        }
    }

    private void FixedUpdate() {
        Run();
    }

    private void Run()
    {
        float targetSpeed = moveInput.x * moveSpeed;
        rb.linearVelocity = new Vector2(targetSpeed, rb.linearVelocity.y);
        if(targetSpeed != 0)
        {
            animator.SetBool("isRunning", true);
            if(targetSpeed > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    private void JumpPressed()
    {
        if(jumpPossible)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpStrength);

            animator.SetBool("isJumping", true);
        }
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
}
