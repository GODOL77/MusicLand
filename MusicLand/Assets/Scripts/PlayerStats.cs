using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    [Header("Basic Stats")]
    public float maxHP = 100f;
    public float currentHP = 100f;

    [Header("Combat Stats")]
    public float attackPower = 10f;
    public float defense = 5f;
    public float attackSpeed = 1f;   // 초당 공격 횟수

    public void TakeDamage(float damage)
    {
        float finalDamage = Mathf.Max(1f, damage - defense);
        currentHP -= finalDamage;

        Debug.Log($"[PlayerStats] 데미지 {finalDamage} 받음 → 현재 HP: {currentHP}");

        if (currentHP <= 0)
            currentHP = 0;
    }

    public bool IsDead()
    {
        return currentHP <= 0;
    }
}
