using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Slider 클래스를 사용하기 위해 필요합니다.

public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider backgroundSlider; // 배경 음악 슬라이더
    [SerializeField] Slider effectSlider;     // 효과음 슬라이더

    // 사운드 관련 키를 상수로 정의하여 오타를 방지합니다.
    private const string BGM_VOLUME_KEY = "BGMVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";

    void Start()
    {
        // PlayerPrefs에 BGMVolume 키가 있는지 확인
        if (!PlayerPrefs.HasKey(BGM_VOLUME_KEY))
        {
            // 키가 없으면 기본값 1 (최대)로 설정
            PlayerPrefs.SetFloat(BGM_VOLUME_KEY, 1f);
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, 1f); // SFX도 기본값 설정
            
            // PlayerPrefs에 값이 저장된 후 불러와서 적용
            LoadVolume();
        }
        else
        {
            // 키가 있으면 기존 값을 불러와서 적용
            LoadVolume();
        }
    }

    // 슬라이더 조작 시 호출되는 함수 (버튼 On Value Changed 이벤트에 연결)
    public void ChangeVolume()
    {
        // 1. AudioListener 볼륨에 슬라이더 값을 적용합니다.
        // *주의: 실제 게임에서는 AudioMixer를 사용하여 BGM과 SFX를 분리해야 합니다.*
        // 여기서는 예시로 BGM 슬라이더 값을 AudioListener에 적용합니다.
        AudioListener.volume = backgroundSlider.value;
        
        // 2. 변경된 값을 저장합니다.
        SaveVolume();
    }

    private void LoadVolume()
    {
        // 저장된 BGM 볼륨 값을 슬라이더와 AudioListener에 적용
        backgroundSlider.value = PlayerPrefs.GetFloat(BGM_VOLUME_KEY);
        AudioListener.volume = backgroundSlider.value; // 로드된 값을 즉시 적용

        // 저장된 SFX 볼륨 값을 슬라이더에 적용
        effectSlider.value = PlayerPrefs.GetFloat(SFX_VOLUME_KEY);
        // *참고: AudioListener.volume은 전체 볼륨이라 SFX에 바로 적용하기는 어렵습니다.*
        
        // 디버깅용:
        Debug.Log("Volume Loaded. BGM: " + backgroundSlider.value + ", SFX: " + effectSlider.value);
    }

    private void SaveVolume()
    {
        // BGM과 SFX에 각각 다른 키를 사용하여 값을 저장합니다. (올바른 방식)
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, backgroundSlider.value);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, effectSlider.value);
        
        // 변경 사항을 디스크에 저장
        PlayerPrefs.Save();
        
        Debug.Log("Volume Saved.");
    }
}