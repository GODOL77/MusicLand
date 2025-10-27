using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    #region 변수선언
    private PlayerMove controls;
    private Vector2 moveInput;
    private Animator playerAnimator;
    private SpriteRenderer playerSpriteRenderer;
    private Rigidbody2D rb;
    #endregion

    // 캐릭터 움직임(속도, 점프) 세팅
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    // 캐릭터 공격지점, BulletManager 불러오기
    [Header("Attack Settings")]
    public Transform firePoint;
    public BulletManager bulletManager;
    // FirePoint의 초기 로컬 위치 저장
    private Vector3 firePointOriginalLocalPos;

    // 캐릭터 기본 바닥여부 -> True
    private bool isGrounded = true;


    // 컴포넌트 가져오기
    private void Awake()
    {
        controls = new PlayerMove();
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();

        // FirePoint 원래 위치 저장
        if (firePoint != null)
            firePointOriginalLocalPos = firePoint.localPosition;

        #region NewInputSystem 세팅
        // 이동 입력 처리
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += _ => moveInput = Vector2.zero;

        // 점프 입력 처리
        controls.Player.Jump.performed += _ => Jump();

        // 공격 입력 처리
        controls.Player.Attack.performed += _ => Attack();
    }

    // Input System 활성화
    private void OnEnable() => controls.Player.Enable();
    private void OnDisable() => controls.Player.Disable();
    #endregion

    private void Update()
    {
        Move();
        UpdateAnimation();
        UpdateFirePointDirection(); // FirePoint 좌우 반전 처리
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        // 좌우 반전
        if (moveInput.x > 0)
            playerSpriteRenderer.flipX = false;
        else if (moveInput.x < 0)
            playerSpriteRenderer.flipX = true;
    }

    private void Jump()
    {
        // isGround일 떄만 점프가 가능하도록 설정
        if (!isGrounded) return;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 땅 충돌판정시 isGrounded false -> true 변경
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.3f)
            isGrounded = true;
    }

    private void UpdateAnimation()
    {
        // 캐릭터 움직이면 Run 애니메이션 재생
        bool isRunning = Mathf.Abs(moveInput.x) > 0.1f;
        playerAnimator.SetBool("isRunning", isRunning);
    }

    private void UpdateFirePointDirection() // 캐릭터 좌우반전 FirePoint에도 적용
    {
        if (firePoint == null) return;

        Vector3 localPos = firePointOriginalLocalPos;
        if (playerSpriteRenderer.flipX)
            localPos.x = -Mathf.Abs(localPos.x);
        else
            localPos.x = Mathf.Abs(localPos.x);

        firePoint.localPosition = localPos;
    }

    private void Attack() // FirePoint 위치에서 총알 발사
    {
        if (bulletManager == null || firePoint == null) return;

        Vector2 fireDir = playerSpriteRenderer.flipX ? Vector2.left : Vector2.right;

        // FireBullet 호출 시 BulletManager의 현재 타입 사용
        bulletManager.FireBullet(firePoint.position, fireDir);
    }
}
