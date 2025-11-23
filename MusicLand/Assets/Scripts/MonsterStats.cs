using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterStats : MonoBehaviour
{
    [Header("Basic Stats")]
    public float maxHP = 50f;
    public float currentHP = 50f;

    [Header("Combat Stats")]
    public float attackPower = 5f;
    public float defense = 2f;

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
