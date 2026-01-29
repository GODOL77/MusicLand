using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    [SerializeField] private float projectileSpeed = 10f;

    [Header("UI References")]
    [SerializeField] private Animator lpAnimator;
    [SerializeField] private UnityEngine.UI.Slider hpSlider; 

    private PlayerStats stats;
    private float horizontal;
    private bool isAttacking = false;
    private bool isDead = false; // 내부 로직용 생사 플래그
    private float targetHP;
    #endregion

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();

        if (stats == null)
        {
            Debug.LogError("[PlayerManager] PlayerStats 컴포넌트를 찾지 못했습니다!");
            return;
        }

        // 게임 시작 시 초기화
        stats.currentHP = stats.maxHP;
        isDead = false;
        
        if (hpSlider != null)
        {
            hpSlider.maxValue = stats.maxHP;
            hpSlider.value = stats.currentHP;
            targetHP = stats.currentHP;
        }

        // [중요] 애니메이터 초기화 (isDie Bool을 false로)
        if (animator != null)
        {
            animator.SetBool("isDie", false);
        }
    }

    private void Update()
    {
        if (isDead) return;

        if (hpSlider != null && stats != null)
        {
            hpSlider.value = stats.currentHP;
        }
    }

    private void FixedUpdate()
    {
        if (stats == null || isDead) return;

        rb.velocity = new Vector2(horizontal * stats.moveSpeed, rb.velocity.y);
        animator.SetBool("isRunning", Mathf.Abs(horizontal) > 0.05f);

        bool grounded = IsGrounded();
        animator.SetBool("isGrounded", grounded);

        if (grounded)
        {
            animator.SetBool("isJumping", false);
        }

        animator.SetBool("isAttacking", isAttacking);
    }

    #region 캐릭터 움직임
    public void Move(InputAction.CallbackContext context)
    {
        if (isDead) { horizontal = 0; return; }

        horizontal = context.ReadValue<Vector2>().x;
        if(horizontal > 0.05f) 
        {
            spriteRenderer.flipX = false;
            attackPoint.localPosition = new Vector3(Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y, attackPoint.localPosition.z);
        }
        else if(horizontal < -0.05f) 
        {
            spriteRenderer.flipX = true;
            attackPoint.localPosition = new Vector3(-Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y, attackPoint.localPosition.z);
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isDead) return;

        if(context.performed && IsGrounded() && stats != null)
        {
            rb.velocity = new Vector2(rb.velocity.x, stats.jumpForce);
            animator.SetBool("isJumping", true);
        }
    }

    private bool IsGrounded()
    {
        if (groundCheck == null) return false;
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.7f, 0.1f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }
    #endregion

    #region 캐릭터 공격
    public void Fire(InputAction.CallbackContext context)
    {
        if (stats == null || isDead) return;

        if (context.started && !isAttacking)
        {
            isAttacking = true;
            animator.SetBool("isAttacking", true);
            InvokeRepeating(nameof(FireProjectile), 0f, stats.lightAttackCooldown);
        }
        else if (context.canceled)
        {
            isAttacking = false;
            animator.SetBool("isAttacking", false);
            CancelInvoke(nameof(FireProjectile));
        }
    }

    private void FireProjectile()
    {
        if (projectilePrefab == null || attackPoint == null || stats == null || isDead) return;

        GameObject projectile = Instantiate(projectilePrefab, attackPoint.position, Quaternion.identity);
        Rigidbody2D projRb = projectile.GetComponent<Rigidbody2D>();
        float direction = spriteRenderer.flipX ? -1f : 1f;
        if (projRb != null)
        {
            projRb.velocity = new Vector2(projectileSpeed * direction, 0f);
        }

        Bullet bulletScript = projectile.GetComponent<Bullet>();
        if(bulletScript != null)
        {
            bulletScript.damage = stats.playerBaseDamage;
        }
    }
    #endregion

    private void OnEnable()
    {
        if (stats != null)
        {
            stats.OnHit += PlayLPAnimaion;
            stats.OnHit += UpdateHPBar;
            stats.OnPlayerDeath += Die;
        }
    }

    private void UpdateHPBar()
    {
        if (stats != null)
        {
            targetHP = stats.currentHP;
        }
    }

    private void OnDisable()
    {
        if (stats != null)
        {
            stats.OnHit -= PlayLPAnimaion;
            stats.OnHit -= UpdateHPBar;
            stats.OnPlayerDeath -= Die;
        }
    }

    private void PlayLPAnimaion()
    {
        if (lpAnimator != null && !isDead)
        {
            lpAnimator.SetTrigger("OnHit"); 
        }
    }

    private void Die()
    {
        if (isDead) return; 
        isDead = true;

        Debug.Log("[PlayerManager] 플레이어 사망!");
        
        rb.velocity = Vector2.zero;
        
        // 애니메이터에 설정된 Bool 파라미터 "isDie"를 true로 설정
        if (animator != null)
        {
            animator.SetBool("isDie", true);
        }

        // LP판 애니메이션 정지 처리 (파라미터가 있다면)
        if (lpAnimator != null)
        {
            lpAnimator.SetBool("isDead", true); 
        }

        CancelInvoke();
        StartCoroutine(RestartAfterDelay());
    }

    IEnumerator RestartAfterDelay()
    {
        Debug.Log("2초 뒤에 GameStart 씬을 로드합니다.");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GameStart");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("Boss"))
        {
            if (stats != null) stats.TakeDamage(10); 
        }
    }
}