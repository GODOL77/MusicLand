// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Beat1Weapon : MonoBehaviour
// {
//     [Header("Weapon Info")]
//     public string weaponName = "Unnamed Beat Weapon";
//     public int beatCount = 1; // 1박자, 2박자, 3박자, 4박자 등

//     [Header("Weapon Stats")]
//     public float baseDamage = 10f;       // 기본 공격력
//     public float damageMultiplier = 1f;  // 박자에 따른 배율
//     public float cooldown = 1f;          // 공격 쿨타임 (박자별로 다르게)

//     [Header("Owner Reference")]
//     public PlayerStats playerStats;      // 플레이어 스탯 참조 (필수)

//     protected bool canAttack = true;

//     // 공통 공격 데미지 계산
//     public virtual float GetDamage()
//     {
//         return (playerStats != null ? playerStats.playerBaseDamage : baseDamage) * damageMultiplier;
//     }

//     // 공격 실행 템플릿
//     public void TryAttack()
//     {
//         if (!canAttack)
//             return;

//         canAttack = false;
//         Attack();

//         Invoke(nameof(ResetCooldown), cooldown);
//     }

//     // 무기별로 실제 공격 로직을 구현하는 함수
//     protected abstract void Attack();

//     private void ResetCooldown()
//     {
//         canAttack = true;
//     }
// }