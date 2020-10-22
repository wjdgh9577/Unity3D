using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPanel : PanelBase
{
    public OptionMode optionMode;

    public override void Initialize()
    {
        this.optionMode.Initialize();
        Show();
    }

    public void OnNewGameButton()
    {
        GUIManager.Instance.messageBoxPanel.CallYesNoMessageBox("Message_NewAccount",
            () =>
            {
                PlayerData.NewAccountSetup();
                PlayerData.SaveData();
                GameManager.Instance.Login();
                Hide();
            }, null);
    }

    public void OnLoadButton()
    {
        if (!PlayerData.LoadData())
        {
            GUIManager.Instance.messageBoxPanel.CallOKMessageBox("Message_NotFoundAccount", null);
            return;
        }
        GameManager.Instance.Login();
        Hide();
    }

    public void OnOptionButton()
    {
        this.optionMode.Show();
    }

    public void OnExitButton()
    {
        GUIManager.Instance.messageBoxPanel.CallYesNoMessageBox("Message_ExitGame",
            () =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }, null);
    }
}
