using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMode : PanelBase
{
    public void OnKoreanButton()
    {
        GUIManager.Instance.messageBoxPanel.CallYesNoMessageBox("LanguageChange",
            () =>
            {
                PlayerPrefs.SetInt("language", (int)Language.Korean);
                PlayerData.SetLanguage();
            },
            () =>
            {

            });
    }

    public void OnEnglishButton()
    {
        GUIManager.Instance.messageBoxPanel.CallYesNoMessageBox("LanguageChange",
            () =>
            {
                PlayerPrefs.SetInt("language", (int)Language.English);
                PlayerData.SetLanguage();
            },
            null);
    }

    public void OnExitButton()
    {
        Hide();
    }
}
