// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class MonsterAI : MonoBehaviour
// {
//     public float moveSpeed = 2f;          // 이동 속도
//     public float attackRange = 1.2f;      // 공격 거리
//     public float attackCooldown = 1f;     // 공격 쿨타임
//     private float lastAttackTime;

//     private Transform player;
//     private MonsterStats stats;

//     private void Start()
//     {
//         player = GameObject.FindGameObjectWithTag("Player").transform;
//         stats = GetComponent<MonsterStats>();
//     }

//     private void Update()
//     {
//         if (stats.IsDead()) return;

//         float dist = Vector2.Distance(transform.position, player.position);

//         // 범위에 들어오면 공격
//         if (dist <= attackRange)
//         {
//             TryAttack();
//         }
//         else
//         {
//             MoveToPlayer();
//         }
//     }

//     private void MoveToPlayer()
//     {
//         if (player == null) return;

//         Vector2 direction = (player.position - transform.position).normalized;
//         transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);

//         // 좌우 반전 (적 방향 보게)
//         GetComponent<SpriteRenderer>().flipX = (player.position.x < transform.position.x);
//     }

//     private void TryAttack()
//     {
//         if (Time.time - lastAttackTime < attackCooldown) return;

//         lastAttackTime = Time.time;

//         // DamageArea 트리거 발동
//         PlayerStats playerStats = player.GetComponent<PlayerManager>().stats;

//         float finalDamage = Mathf.Max(1f, stats.attackPower - playerStats.defense);
//         playerStats.TakeDamage(finalDamage);

//         Debug.Log($"몬스터가 플레이어를 공격! 데미지 {finalDamage}");
//     }
// }
