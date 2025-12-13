using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    [Header("GameObject Recognition")]
    public GameObject operationUiPanel;
    public GameObject soundUiPanel;
    public GameObject graphicsUiPanel;

    [Header("GameObject Recognition Toggle")]
    public bool isOperationUiPaneActive = false;
    public bool isSoundUiPanelActive = false;
    public bool isGraphicsUiPanelActive = false;

    void Start()
    {
        HideAllContentPanels();
    }

    void Update()
    {
        
    }


    public void OnClick_ShowOperationPanel()
    {
        Time.timeScale = 0f;
        operationUiPanel.SetActive(true);
        isOperationUiPaneActive = true;



        soundUiPanel.SetActive(false);
        isSoundUiPanelActive = false;

        graphicsUiPanel.SetActive(false);
        isGraphicsUiPanelActive = false;
        
        Debug.Log("Switched to: Operation Panel");
    }

    public void OnClick_ShowSoundPanel()
    {
        soundUiPanel.SetActive(true);
        isSoundUiPanelActive = true;
        Time.timeScale = 0f;



        operationUiPanel.SetActive(false);
        isOperationUiPaneActive = false;
        
        graphicsUiPanel.SetActive(false);
        isGraphicsUiPanelActive = false;
        
        Debug.Log("Switched to: Sound Panel");
    }

    // 3. 그래픽 Panel을 활성화하고 나머지 Panel을 비활성화하는 함수
    public void OnClick_ShowGraphicsPanel()
    {
        graphicsUiPanel.SetActive(true);
        isGraphicsUiPanelActive = true;
        Time.timeScale = 0f;



        operationUiPanel.SetActive(false);
        isOperationUiPaneActive = false;

        soundUiPanel.SetActive(false);
        isSoundUiPanelActive = false;
        
        Debug.Log("Switched to: Graphics Panel");
    }

    public void HideAllContentPanels()
    {
        operationUiPanel.SetActive(false);
        isOperationUiPaneActive = false;
        
        soundUiPanel.SetActive(false);
        isSoundUiPanelActive = false;
        
        graphicsUiPanel.SetActive(false);
        isGraphicsUiPanelActive = false;
        Debug.Log("All Content Panels Hidden.");
    }
}
