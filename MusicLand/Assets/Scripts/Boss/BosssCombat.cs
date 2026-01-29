using UnityEngine;

public class BossCombat : MonoBehaviour
{
    public float touchDamage = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 보스 몸에 플레이어가 닿았을 때
        if (collision.CompareTag("Player"))
        {
            PlayerStats stats = collision.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.TakeDamage(touchDamage);
                Debug.Log("보스가 플레이어를 공격!");
            }
        }
    }
}