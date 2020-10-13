using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OptionMode : PanelBase
{
    public static Action languageChange;

    public void OnKoreanButton()
    {
        GUIManager.Instance.messageBoxPanel.CallYesNoMessageBox("Message_LanguageChange",
            () =>
            {
                PlayerPrefs.SetInt("language", (int)Language.Korean);
                PlayerData.SetLanguage();
                languageChange?.Invoke();
            },
            () =>
            {

            });
    }

    public void OnEnglishButton()
    {
        GUIManager.Instance.messageBoxPanel.CallYesNoMessageBox("Message_LanguageChange",
            () =>
            {
                PlayerPrefs.SetInt("language", (int)Language.English);
                PlayerData.SetLanguage();
                languageChange?.Invoke();
            },
            null);
    }

    public void OnExitButton()
    {
        Hide();
    }
}
