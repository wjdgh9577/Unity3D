using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPanel : PanelBase
{
    public void OnNewGameButton()
    {
        GameManager.Instance.Login();
        Hide();
    }

    public void OnLoadButton()
    {
        Debug.LogError("세이브/로드 구현 필요!");
    }

    public void OnExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
