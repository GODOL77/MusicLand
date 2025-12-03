using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // 특정 씬으로 이동
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // 메인 게임으로 이동
    public void GoMainGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    // 타이틀 화면으로 이동
    public void GoTitle()
    {
        SceneManager.LoadScene("Title");
    }

    // 게임 종료
    public void QuitGame()
    {
        Debug.Log("게임 종료!");
        Application.Quit();
    }
}
