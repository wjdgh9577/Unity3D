using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPanel : PanelBase
{
    public OptionMode optionMode;

    public void OnNewGameButton()
    {
        GUIManager.Instance.messageBoxPanel.CallYesNoMessageBox("NewAccount",
            () =>
            {
                PlayerData.NewAccountSetup();
                PlayerData.SaveData();
                GameManager.Instance.Login();
                Hide();
            });
    }

    public void OnLoadButton()
    {
        if (!PlayerData.LoadData())
        {
            GUIManager.Instance.messageBoxPanel.CallOKMessageBox("NotFoundAccount");
            return;
        }
        GameManager.Instance.Login();
        Hide();
    }

    public void OnOptionButton()
    {
        optionMode.Show();
    }

    public void OnExitButton()
    {
        GUIManager.Instance.messageBoxPanel.CallYesNoMessageBox("ExitGame",
            () =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            });
    }
}
