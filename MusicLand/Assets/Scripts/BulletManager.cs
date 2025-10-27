using UnityEngine;

public class BulletManager : MonoBehaviour
{
    #region 총알 기본 설정
    public enum BulletType { Slow, Fast }

    [Header("Bullet Prefabs")]
    public GameObject slowBulletPrefab;
    public GameObject fastBulletPrefab;

    [Header("Bullet Settings")]
    public float slowBulletSpeed = 3f;
    public float fastBulletSpeed = 7f;
    public float slowBulletLifetime = 3f;
    public float fastBulletLifetime = 1.5f;

    // 현재 발사할 총알 타입 관리
    private BulletType currentBulletType = BulletType.Slow;
    #endregion

    // 총알 생성 함수
    public void FireBullet(Vector2 position, Vector2 direction)
    {
        FireBullet(position, direction, currentBulletType);
    }

    public void FireBullet(Vector2 position, Vector2 direction, BulletType type)
    {
        GameObject prefab = type == BulletType.Fast ? fastBulletPrefab : slowBulletPrefab;
        if (prefab == null)
        {
            Debug.LogWarning("Bullet Prefab 없음");
            return;
        }

        GameObject bulletObj = Instantiate(prefab, position, Quaternion.identity);
        BulletBehaviour behaviour = bulletObj.AddComponent<BulletBehaviour>();
        behaviour.Initialize(type, direction, this);
    }

    // 내부 클래스 (Bullet 역할)
    public class BulletBehaviour : MonoBehaviour
    {
        private BulletType type;
        private Vector2 direction;
        private float speed;
        private float lifetime;
        private Rigidbody2D rb;
        private BulletManager manager;

        public void Initialize(BulletType bulletType, Vector2 dir, BulletManager mgr)
        {
            type = bulletType;
            direction = dir.normalized;
            manager = mgr;

            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.freezeRotation = true;

            switch (type)
            {
                case BulletType.Slow:
                    speed = manager.slowBulletSpeed;
                    lifetime = manager.slowBulletLifetime;
                    break;
                case BulletType.Fast:
                    speed = manager.fastBulletSpeed;
                    lifetime = manager.fastBulletLifetime;
                    break;
            }

            rb.linearVelocity = direction * speed;
            Destroy(gameObject, lifetime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Wall") || collision.CompareTag("Enemy"))
            {
                Destroy(gameObject);
            }
        }
    }

    // 총알 타입 변경 (UI Button에서 호출 가능)
    public void ChangeBullet()
    {
        currentBulletType = currentBulletType == BulletType.Slow ? BulletType.Fast : BulletType.Slow;
        Debug.Log("Current Bullet changed to: " + currentBulletType);
    }

    // 현재 총알 타입 반환 (필요 시)
    public BulletType GetCurrentBulletType()
    {
        return currentBulletType;
    }
}
