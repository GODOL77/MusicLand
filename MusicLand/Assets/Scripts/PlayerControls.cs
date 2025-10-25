using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    private PlayerMove controls;    // InputActions 클래스
    private Vector2 moveInput;
    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    private bool isGrounded = true;

    private void Awake()
    {
        controls = new PlayerMove();
        rb = GetComponent<Rigidbody2D>();

        // 이동 입력 처리
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += _ => moveInput = Vector2.zero;

        // 점프 입력 처리
        controls.Player.Jump.performed += _ => Jump();
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
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
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
}
