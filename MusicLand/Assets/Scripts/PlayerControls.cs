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
    private Vector3 firePointOriginalLocalPos;

    private bool isGrounded = true;

    // 박자 관련
    private float lastBeatTime;
    [Header("Rhythm Settings")]
    public float beatTolerance = 0.15f; 

    private void Awake()
    {
        controls = new PlayerMove();
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();

        if (firePoint != null)
            firePointOriginalLocalPos = firePoint.localPosition;

        #region NewInputSystem 세팅
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += _ => moveInput = Vector2.zero;

        controls.Player.Jump.performed += _ => Jump();
        controls.Player.Attack.performed += _ => Attack();
        #endregion
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        BeatManager.OnBeat += OnBeat;
    }

    private void OnDisable()
    {
        controls.Player.Disable();
        BeatManager.OnBeat -= OnBeat;
    }

    private void Update()
    {
        Move();
        UpdateAnimation();
        UpdateFirePointDirection();
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        if (moveInput.x > 0)
            playerSpriteRenderer.flipX = false;
        else if (moveInput.x < 0)
            playerSpriteRenderer.flipX = true;
    }

    private void Jump()
    {
        if (!isGrounded) return;

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.3f)
            isGrounded = true;
    }

    private void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(moveInput.x) > 0.1f;
        playerAnimator.SetBool("isRunning", isRunning);
    }

    private void UpdateFirePointDirection()
    {
        if (firePoint == null) return;

        Vector3 localPos = firePointOriginalLocalPos;

        if (playerSpriteRenderer.flipX)
            localPos.x = -Mathf.Abs(localPos.x);
        else
            localPos.x = Mathf.Abs(localPos.x);

        firePoint.localPosition = localPos;
    }

    private void OnBeat()
    {
        lastBeatTime = Time.time;
    }

    private void Attack()
    {
        if (bulletManager == null || firePoint == null) return;

        float timeSinceBeat = Mathf.Abs(Time.time - lastBeatTime);  // timeSinceBeat 오류 수정
        Vector2 fireDir = playerSpriteRenderer.flipX ? Vector2.left : Vector2.right;

        // beatToleranceTolearance 오타 수정 → beatTolerance
        if (timeSinceBeat <= beatTolerance)
        {
            bulletManager.FireBullet(firePoint.position, fireDir, BulletManager.BulletType.Fast);
            Debug.Log("Nice Timing");
        }
        else
        {
            bulletManager.FireBullet(firePoint.position, fireDir, BulletManager.BulletType.Slow);
            Debug.Log("Normal Attack!");
        }
    }
}
