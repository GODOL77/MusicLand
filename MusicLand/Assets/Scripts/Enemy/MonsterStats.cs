using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterStats : MonoBehaviour
{
    [Header("Health")]
    public float currentHP = 50f;
    public float maxHP = 100f;

    [Header("Combat Stats")]
    public float attackPower = 5f;
    public float defense = 2f;

    [Header("Movement Stats")]
    public float moveSpeed = 10f;
    public float jumpForce = 7f;

    [Header("AttakcCooldown")]
    public float AttackCooldown = 1f;

    public void TakeDamage(float damage)
    {
        float finalDamage = Mathf.Max(1f, damage - defense);
        currentHP -= finalDamage;

        Debug.Log($"[MonsterStats] 데미지 {finalDamage} 받음 → 현재 HP: {currentHP}");

        if (currentHP <= 0)
            Die();
    }

    public bool IsDead()
    {
        return currentHP <= 0;
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} 사망!");
        Destroy(gameObject);
    }
}
