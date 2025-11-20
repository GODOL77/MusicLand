using UnityEngine;

public class BeatManager : MonoBehaviour
{
    [Header("BPM Settings")]
    public float bpm = 120f;
    public static float BeatInterval;   // 원래 선언 유지
    private float timer;

    public static event System.Action OnBeat;

    void Start()
    {
        BeatInterval = 60f / bpm;   // 오타 수정 (beatInterval → BeatInterval)
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;   // timer.deltaTime 오타 수정

        if (timer >= BeatInterval)
        {
            timer -= BeatInterval;
            OnBeat?.Invoke();      // Onbeat → OnBeat (오타 수정)
        }
    }
}
