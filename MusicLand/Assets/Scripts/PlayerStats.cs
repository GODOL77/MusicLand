using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
#region 플레이어 스탯관련
public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public float currentHP = 100f;
    public float maxHP = 100f;

    [Header("Combat Stats")]
    public float playerBaseDamage = 10f;
    public float playerDefence = 5f;

    [Header("Movement Stats")]
    public float moveSpeed = 10f;
    public float jumpForce = 7f;

    [Header("AttakcCooldown")]
    // 약공격, 강공격 쿨타임 관련 변수
    public float lightAttackCooldown = 1f;
    public float strongAttackCooldown = 3f;

    // 멀티플라이어
    private const float LightAttackMultiplier = 1.25f;
    private const float StrongAttackMultiplier = 1.5f;

    #endregion

    // 사망 이벤트 처리
    public event Action OnPlayerDeath;

    // 플레이어 데미지 계산식
    public void TakeDamage(float damage)
    {
        float finalDamage = damage - playerDefence;   // 나중에 계산식 직접 만들어서 수정할 것
        currentHP -= finalDamage;
        Debug.Log($"[PlayerStats] 데미지 {finalDamage} 받음 → 현재 HP: {currentHP}");
        if (currentHP <= 0)
        {
            currentHP = 0;
            OnPlayerDeath?.Invoke();
        }
    }

    public bool IsDead()
    {
        return currentHP <= 0;
    }

    // 실제 공격력 계산 함수
    public float GetLightAttackDamage()
    {
        return playerBaseDamage * LightAttackMultiplier;
    }

    public float GetStrongAttackDamage()
    {
        return playerBaseDamage * StrongAttackMultiplier;
    }
}
