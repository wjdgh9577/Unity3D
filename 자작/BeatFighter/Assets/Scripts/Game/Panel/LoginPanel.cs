using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPanel : PanelBase
{
    public void OnNewGameButton()
    {
        GUIManager.Instance.messageBoxPanel.CallYesNoMessageBox(
            () => 
            {
                PlayerData.NewAccountSetup();
                PlayerData.SaveData();
                GameManager.Instance.Login();
                Hide();
            }, 
            () => 
            {

            });
    }

    public void OnLoadButton()
    {
        if (!PlayerData.LoadData())
        {
            GUIManager.Instance.messageBoxPanel.CallOKMessageBox(
                () =>
                {

                });
            return;
        }
        GameManager.Instance.Login();
        Hide();
    }

    public void OnExitButton()
    {
        GUIManager.Instance.messageBoxPanel.CallYesNoMessageBox(
            () =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            },
            () =>
            {

            });
    }
}
