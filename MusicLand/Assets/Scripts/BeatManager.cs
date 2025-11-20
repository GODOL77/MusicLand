using UnityEngine;

public class BeatManager : MonoBehaviour
{
    [Header("BPM Settings")]
    public float bpm = 120f;
    public static float BeatInterval;
    private float timer;

    public static event System.Action OnBeat;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        beatInterval = 60f / bpm;
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += timer.deltaTime;
        if (timer >= beatInterval)
        {
            timer -= beatInterval;
            Onbeat?.Invoke();   // 박자 발생 알림
        }
    }
}
