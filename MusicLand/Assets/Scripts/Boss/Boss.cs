using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 씬 이동을 위해 필수

public class Boss : MonoBehaviour 
{
    [Header("Settings")]
    public Transform player;      
    public float dashSpeed = 12f;
    public float maxHP = 100f;
    public float currentHP;
    public float touchDamage = 10f; 

    [Header("UI References")]
    [SerializeField] private Slider bossHPBar; // 보스 체력 바 슬라이더 연결

    private SpriteRenderer spriteRenderer;

    void Awake() 
    {
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (bossHPBar != null)
        {
            bossHPBar.maxValue = maxHP;
            bossHPBar.value = maxHP;
        }
    }

    void Start() 
    { 
        if (player == null) 
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if(playerObj != null) player = playerObj.transform;
        }
        StartCoroutine(Pattern()); 
    }

    IEnumerator Pattern()
    {
        while(true) {
            yield return new WaitForSeconds(2f); 
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.3f);
            
            if(player != null)
            {
                Vector2 dir = (player.position - transform.position).normalized;
                float startTime = Time.time;
                while(Time.time < startTime + 0.4f) { 
                    transform.Translate(dir * dashSpeed * Time.deltaTime);
                    yield return null;
                }
            }
            spriteRenderer.color = Color.gray; 
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        
        if (bossHPBar != null)
        {
            bossHPBar.value = Mathf.Max(0, currentHP);
        }

        Debug.Log($"보스 체력: {currentHP}");
        StartCoroutine(HitEffect());

        if (currentHP <= 0)
        {
            Die();
        }
    }

    IEnumerator HitEffect()
    {
        spriteRenderer.color = Color.gray;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.gray;
    }

    private void Die()
    {
        Debug.Log("보스 처치! Clear 씬으로 이동합니다.");
        
        // 모든 로직 중지
        StopAllCoroutines();

        // [핵심] "Clear"라는 이름의 씬으로 이동
        // 유니티 프로젝트 창에 있는 씬 이름과 정확히 일치해야 합니다.
        SceneManager.LoadScene("Clear");

        Destroy(gameObject); 
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) { 
            PlayerStats stats = collision.GetComponent<PlayerStats>();
            if(stats != null) stats.TakeDamage(touchDamage);
        }
    }
}