using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 3f;
    public float damage = 10f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 보스와 충돌 시 (Boss 클래스 참조)
        if (collision.CompareTag("Boss"))
        {
            Boss bossScript = collision.GetComponent<Boss>();
            if (bossScript != null)
            {
                bossScript.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

        // 일반 몬스터와 충돌 시
        if (collision.CompareTag("Enemy"))
        {
            // MonsterStats가 있다면 호출 (없으면 무시됨)
            collision.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
    }

            // 벽이나 땅
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // 바닥이나 벽은 Is Trigger가 꺼져있으므로 이 함수로 들어옵니다.
            if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground"))
            {
                Debug.Log("일반 충돌로 벽/바닥 감지!");
                Destroy(gameObject);
            }
        }
}