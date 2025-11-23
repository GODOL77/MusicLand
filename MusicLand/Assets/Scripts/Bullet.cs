using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 4f;    // Bullet 생존 시간
    public float damage = 10f;     // 데미지

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 벽이나 땅과 충돌 시 Bullet 삭제
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        // 몬스터와 충돌 시 데미지 적용 후 Bullet 삭제
        if (collision.gameObject.CompareTag("Enemy"))
        {
            MonsterStats monster = collision.gameObject.GetComponent<MonsterStats>();
            if (monster != null)
            {
                monster.TakeDamage(damage);
            }
            gameObject.SetActive(false);
        }
    }
}
