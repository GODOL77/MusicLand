using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    #region inspector 및 변수정리

    [Header("Player Component References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Grounding")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;

    [Header("Attack")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float projectileSpeed = 10f;   // 발사체 속도

    // PlayerStats 변수 가져오기
    private PlayerStats stats;
    private float horizontal;
    private bool isAttacking = false;
    #endregion

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();

        if (stats == null)
            Debug.LogError("[PlayerManager] PlayerStats 컴포넌트를 찾지 못했습니다!");
    }

    private void FixedUpdate()
    {
        // 이동 처리
        rb.velocity = new Vector2(horizontal * stats.moveSpeed, rb.velocity.y);

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

    #region 캐릭터 움직임
    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        // 이동 방향에 따라 캐릭터 좌우 뒤집기
        if(horizontal > 0.05f) 
        {
            spriteRenderer.flipX = false;  // 캐릭터 X축반전 해제
            attackPoint.localPosition = new Vector3(Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y, attackPoint.localPosition.z);  // 캐릭터 공격포인트 X축반전 해제
        }
        else if(horizontal < -0.05f) 
        {
            spriteRenderer.flipX = true; // 캐릭터 X축반전
            attackPoint.localPosition = new Vector3(-Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y, attackPoint.localPosition.z); // 캐릭터 공격포인트 X축반전
        }
    }


    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, stats.jumpForce);  // PlayerStats 변수사용
            animator.SetBool("isJumping", true);
        }
    }

    // 땅인지 체크하는 함수
    private bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.7f, 0.1f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }
    #endregion

#region 캐릭터 공격
    public void Fire(InputAction.CallbackContext context)
    {
        if (context.started && !isAttacking)
        {
            isAttacking = true;
            animator.SetBool("isAttacking", true);
            InvokeRepeating(nameof(FireProjectile), 0f, stats.lightAttackCooldown); // 반복 발사 시작
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
    if (projRb != null)
    {
        projRb.velocity = new Vector2(projectileSpeed * direction, 0f);
    }

    // Bullet 스크립트에 플레이어 공격력 적용
    Bullet bulletScript = projectile.GetComponent<Bullet>();
    if(bulletScript != null)
    {
        bulletScript.damage = stats.playerBaseDamage;
    }
}

    #endregion

    #region 플레이어 사망 처리
    private void Die()
    {
        Debug.Log("[PlayerManager] 플레이어 사망!");

        rb.velocity = Vector2.zero;
        animator.SetTrigger("isDie");

        // 모든 행동 중지
        CancelInvoke();
        this.enabled = false;
    }
    #endregion
}
