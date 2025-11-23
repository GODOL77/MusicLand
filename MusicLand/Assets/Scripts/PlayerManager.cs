using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    #region inspector 정리

    [Header("Player Component References")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;

    [Header("Player Settings")]
    [SerializeField] float speed;
    [SerializeField] float jumpingPower;

    [Header("Grounding")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    #endregion

    private float horizontal;

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
    }

    #region 캐릭터 움직임 관련 정리
    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
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

    public void Attack(InputAction.CallbackContext context)
    {
        
    }
}
