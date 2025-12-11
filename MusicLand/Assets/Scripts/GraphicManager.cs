using UnityEngine;
using UnityEngine.UI; // Toggle 컴포넌트를 사용하기 위해 필요합니다.

public class GraphicsManager : MonoBehaviour
{
    // Inspector에서 Toggle 컴포넌트를 연결할 변수입니다.
    [Header("UI Toggles")]
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle vsyncToggle;

    // PlayerPrefs에 저장될 키를 정의합니다.
    private const string FULLSCREEN_KEY = "IsFullscreen";
    private const string VSYNC_KEY = "IsVSyncEnabled";

    void Start()
    {
        // 1. 저장된 설정을 불러와 UI에 반영합니다.
        LoadGraphicsSettings();
        
        // 2. UI 토글과 함수를 연결합니다.
        // *주의: 인스펙터 창에서 직접 연결하거나, 아래처럼 스크립트로 연결해야 합니다.*
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        vsyncToggle.onValueChanged.AddListener(SetVSync);
    }

    private void LoadGraphicsSettings()
    {
        // 1. 전체 화면 설정 불러오기
        // ************************************************************
        // 수정된 부분: 저장된 값이 없으면 기본값 (1: 전체 화면)을 사용합니다.
        // ************************************************************
        int fullscreenValue = PlayerPrefs.GetInt(FULLSCREEN_KEY, 1); 
        bool isFullscreen = (fullscreenValue == 1);
        
        // UI 토글 상태 및 실제 화면 설정 적용
        fullscreenToggle.isOn = isFullscreen;
        Screen.fullScreen = isFullscreen; 
        
        // 2. 수직 동기화 설정 불러오기
        // ************************************************************
        // 수정된 부분: 저장된 값이 없으면 기본값 (0: 비활성화)을 사용합니다.
        // ************************************************************
        int vsyncValue = PlayerPrefs.GetInt(VSYNC_KEY, 0);
        bool isVSyncEnabled = (vsyncValue == 1);
        
        // UI 토글 상태 및 실제 VSync 설정 적용
        vsyncToggle.isOn = isVSyncEnabled;
        // QualitySettings.vSyncCount: 0=Disabled, 1=VSync, 2=Every Second Vsync
        QualitySettings.vSyncCount = isVSyncEnabled ? 1 : 0; 
        
        Debug.Log("Graphics Settings Loaded. Fullscreen Default: " + isFullscreen + ", VSync Default: " + isVSyncEnabled);
    }

    // --- 버튼/토글 클릭 시 호출되는 함수 ---

    // 전체 화면 설정 함수
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt(FULLSCREEN_KEY, isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
        
        Debug.Log($"Fullscreen set to: {isFullscreen}");
    }

    // 수직 동기화 설정 함수
    public void SetVSync(bool isEnabled)
    {
        QualitySettings.vSyncCount = isEnabled ? 1 : 0;
        PlayerPrefs.SetInt(VSYNC_KEY, isEnabled ? 1 : 0);
        PlayerPrefs.Save();
        
        Debug.Log($"VSync set to: {isEnabled}");
    }
}