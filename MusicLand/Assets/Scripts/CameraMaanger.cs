using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Player Settings")]
    public Transform player;                  // 따라갈 플레이어
    public SpriteRenderer mapRenderer;        // 맵 스프라이트

    [Header("Pause Menu Settings")]
    // 🔔 인스펙터 창에서 활성화/비활성화할 Pause Menu Panel을 연결합니다.
    public GameObject pauseMenuPanel;
    private bool isPaused = false;

    [Header("External UI")]
    public GameObject settingsUIPanel;

    private float minX, maxX, minY, maxY;

    void Awake()
    {
        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);
        Debug.Log("Game Resumed");
    }

    void Start()
    {
        // 카메라의 절반 크기 계산
        float camHeight = Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect;

        // 맵(스프라이트)의 실제 월드 경계값 읽기
        Bounds mapBounds = mapRenderer.bounds;

        minX = mapBounds.min.x + camWidth;
        maxX = mapBounds.max.x - camWidth;

        minY = mapBounds.min.y + camHeight;
        maxY = mapBounds.max.y - camHeight;
    }

    void Update()
    {
        // 🚨 수정된 부분: ESC 키는 현재 Menu가 열려있을 때만 Toggle 함수를 호출
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ESC를 누르면, 메뉴가 열려있다면 닫고 시간을 재개합니다.
            // (Pause Menu가 설정 UI 위에서 최상단으로 뜬다고 가정)
            if (isPaused || (settingsUIPanel != null && settingsUIPanel.activeSelf))
            {
                 // 어떤 메뉴든 닫고 게임을 재개하는 함수 호출
                ResumeGame();
            }
            else
            {
                 // 게임이 진행 중이라면 Pause Menu를 엽니다.
                ShowPauseMenu();
            }
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        // 플레이어 위치값 가져오기
        Vector3 targetPos = player.position;

        // Clamp(카메라가 맵 밖으로 못 나가게)
        float clampedX = Mathf.Clamp(targetPos.x, minX, maxX);
        float clampedY = Mathf.Clamp(targetPos.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    public void ShowPauseMenu()
    {
        if (pauseMenuPanel != null && !isPaused)
        {
            pauseMenuPanel.SetActive(true);
            isPaused = true;
            Time.timeScale = 0f; // 시간 정지
            Debug.Log("Game Paused: Pause Menu Shown.");
        }
    }

    public void ResumeGame()
    {
        // 현재 열려있는 모든 UI를 닫습니다. (설정 UI 포함)
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        if (settingsUIPanel != null)
        {
            settingsUIPanel.SetActive(false);
        }

        isPaused = false;
        Time.timeScale = 1f; // 시간 재개
        Debug.Log("Game Resumed.");
    }

    public void OpenSettingsFromPause()
    {
        if (settingsUIPanel != null)
        {
            // Pause Menu Panel을 닫고 설정 Panel을 엽니다.
            pauseMenuPanel.SetActive(false);
            settingsUIPanel.SetActive(true);
            isPaused = false; // Pause Menu는 닫힘
            
            // Time.timeScale은 여전히 0f를 유지합니다.
            Debug.Log("Switched to Settings. Time remains stopped.");
        }
    }
}
