using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    public float damage = 10f;       // 공격력
    public string targetTag = "Enemy"; // 공격할 대상 태그

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            // 플레이어가 공격하면 몬스터 데미지
            MonsterStats monster = collision.GetComponent<MonsterStats>();
            if(monster != null)
            {
                monster.TakeDamage(damage);
            }

            // 몬스터 공격이면 플레이어 데미지
            PlayerStats player = collision.GetComponent<PlayerStats>();
            if(player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}
