using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    #region 설정
    private PlayerMove controls;    // InputActions 클래스
    private Vector2 moveInput;
    private Animator playerAnimator;    // 애니메이터 클래스 설정
    private SpriteRenderer playerSpriteRenderer;
    private Rigidbody2D rb;
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    #endregion

    private bool isGrounded = true;

    private void Awake()
    {
        controls = new PlayerMove();
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        // 이동 입력 처리
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += _ => moveInput = Vector2.zero;

        // 점프 입력 처리
        controls.Player.Jump.performed += _ => Jump();

        // SpriteRenderer 가져오기
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Update()
    {
        Move();
        UpdateAnimation();
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        // 좌우반전
        if (moveInput.x > 0)
        {
            playerSpriteRenderer.flipX = false; // 오른쪽 이동 → 기본 방향
        }
        else if (moveInput.x < 0)
        {
            playerSpriteRenderer.flipX = true;  // 왼쪽 이동 → 반전
        }
    }

    private void Jump()
    {
        if (!isGrounded) return;
        {

            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }
    // 땅과 충돌 시 점프 기능 상태로 복구
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.3f)
        {
            isGrounded = true;
        }
    }

    private void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(moveInput.x) > 0.1f;

        playerAnimator.SetBool("isRunning", isRunning);
    }
}
