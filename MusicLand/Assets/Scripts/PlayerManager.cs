using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    #region inspector 및 변수정리

    [Header("Player Component References")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("Player Settings")]
    [SerializeField] float speed;
    [SerializeField] float jumpingPower;

    [Header("Grounding")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;

    [Header("Stats")]
    public PlayerStats stats = new PlayerStats();

    [Header("Attack")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform attackPoint;
    [SerializeField] float projectileSpeed = 10f;   // 발사체 속도
    [SerializeField] float attackCooldown = 0.3f;  // 발사 쿨타임
    private bool isAttacking = false;

    private float horizontal;
    #endregion

    private void FixedUpdate()
    {
        // 이동 처리
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        // Animator 관리
        animator.SetBool("isRunning", Mathf.Abs(horizontal) > 0.05f);

        // isGrounded 체크
        bool grounded = IsGrounded();
        animator.SetBool("isGrounded", grounded);

        // 착지시 점프 종료
        if (grounded)
        {
            animator.SetBool("isJumping", false);
        }

        // 공격 상태 Animator에 반영
        animator.SetBool("isAttacking", isAttacking);
        
    }
    private void Update()
    {
        // 플레이어 사망 처리
        if (stats.IsDead())
        {
            Die();
        }
    }

    #region 캐릭터 움직임 관련 정리
    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        // 🔹 이동 방향에 따라 캐릭터 좌우 뒤집기
        if(horizontal > 0.05f) 
        {
            spriteRenderer.flipX = false;  // 오른쪽
            attackPoint.localPosition = new Vector3(Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y, attackPoint.localPosition.z);
        }
        else if(horizontal < -0.05f) 
        {
            spriteRenderer.flipX = true; // 왼쪽
            attackPoint.localPosition = new Vector3(-Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y, attackPoint.localPosition.z);
        }
    }


    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            animator.SetBool("isJumping", true);
        }
    }

    // 땅인지 체크하는 함수
    private bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.7f, 0.1f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }
    #endregion

#region 공격 관련 정리 (Invoke 반복 발사)
    public void Fire(InputAction.CallbackContext context)
    {
        if (context.started && !isAttacking)
        {
            isAttacking = true;
            animator.SetBool("isAttacking", true);
            InvokeRepeating(nameof(FireProjectile), 0f, attackCooldown); // 반복 발사 시작
        }
        else if (context.canceled) // 버튼 떼면 공격 종료
        {
            isAttacking = false;
            animator.SetBool("isAttacking", false);
            CancelInvoke(nameof(FireProjectile)); // 반복 발사 종료
        }
    }

    private void FireProjectile()
{
    if (projectilePrefab == null || attackPoint == null) return;

    GameObject projectile = Instantiate(projectilePrefab, attackPoint.position, Quaternion.identity);
    Rigidbody2D projRb = projectile.GetComponent<Rigidbody2D>();
    float direction = spriteRenderer.flipX ? -1f : 1f;
    if (projRb != null) projRb.velocity = new Vector2(projectileSpeed * direction, 0f);

    // Bullet 스크립트에 플레이어 공격력 적용
    Bullet bulletScript = projectile.GetComponent<Bullet>();
    if(bulletScript != null)
    {
        bulletScript.damage = stats.attackPower;
    }
}

    #endregion

    #region 플레이어 사망 처리
    private void Die()
    {
        Debug.Log("플레이어 사망!");
        animator.SetTrigger("isDie");  // 사망 애니메이션 트리거
        rb.velocity = Vector2.zero;
        this.enabled = false; // 플레이어 컨트롤 종료
    }
    #endregion
}
