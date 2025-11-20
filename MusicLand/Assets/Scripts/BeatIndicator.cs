using UnityEngine;
using UnityEngine.UI;
using System.Collections;
    
public class BeatIndicator : MonoBehaviour
{
    [Header("박자 UI")]
    public Image beatCircle;

    void OnEnable() => BeatManager.OnBeat += OnBeat;
    void OnDisable() => BeatManager.OnBeat -= OnBeat;

    void OnBeat()
    {
        StopAllCoroutines();
        StartCoroutine(PulseEffect());
    }

    IEnumerator PulseEffect()
    {
        beatCircle.transform.localScale = Vector3.one * 1.3f;
        beatCircle.color = Color.white;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            beatCircle.transform.localScale = Vector3.Lerp(Vector3.one * 1.3f, Vector3.one, t);
            beatCircle.color = Color.Lerp(Color.white, Color.gray, t);
            yield return null;
        }
    }
}
