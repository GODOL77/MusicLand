using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMoving : MonoBehaviour
{
    public GameObject TempUI;

    // 이 함수를 버튼의 OnClick 이벤트에 연결하세요.
    public void TempPanel()
    {
        if (TempUI != null)
        {
            // 현재 상태의 반대로 설정 (true -> false / false -> true)
            bool isActive = TempUI.activeSelf;
            TempUI.SetActive(!isActive);
        }
    }
}