using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OptionMode : PanelBase
{
    public static Action languageChange;

    [SerializeField]
    private Scrollbar musicScroll;
    [SerializeField]
    private Text musicDegree;
    [SerializeField]
    private Scrollbar effectScroll;
    [SerializeField]
    private Text effectDegree;

    public override void Initialize()
    {
        
    }

    public override void Show()
    {
        base.Show();
        Setup();
    }

    private void Setup()
    {
        musicScroll.value = PlayerData.musicSoundDegree;
        effectScroll.value = PlayerData.effectSoundDegree;
    }

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

    public void OnMusicScrool()
    {
        PlayerPrefs.SetFloat("musicSoundDegree", musicScroll.value);
        PlayerData.SetMusicSoundDegree();
        AudioManager.Instance.SetMusicSoundDegree();
        musicDegree.text = Mathf.RoundToInt(musicScroll.value * 100) + "%";
    }

    public void OnEffectScrool()
    {
        PlayerPrefs.SetFloat("effectSoundDegree", effectScroll.value);
        PlayerData.SetEffectSoundDegree();
        AudioManager.Instance.SetEffectSoundDegree();
        effectDegree.text = Mathf.RoundToInt(effectScroll.value * 100) + "%";
    }
}
