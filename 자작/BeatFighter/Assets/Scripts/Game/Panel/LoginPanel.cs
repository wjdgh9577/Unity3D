using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPanel : PanelBase
{
    public void OnNewGameButton()
    {
        PlayerData.NewAccountSetup();
        PlayerData.SaveData();
        GameManager.Instance.Login();
        Hide();
    }

    public void OnLoadButton()
    {
        if (!PlayerData.LoadData()) return;
        GameManager.Instance.Login();
        Hide();
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
