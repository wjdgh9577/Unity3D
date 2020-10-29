using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : Singleton<GUIManager>
{
    public CombatPanel combatPanel;
    public LoginPanel loginPanel;
    public MenuPanel menuPanel;
    public CollectionPanel collectionPanel;
    public MessageBoxPanel messageBoxPanel;

    [SerializeField]
    private Image fadeScreen;
    [SerializeField]
    private Image loadingScreen;
    [SerializeField]
    private Text loadingText;

    public void Initialize()
    {
        this.combatPanel.Initialize();
        this.loginPanel.Initialize();
        this.menuPanel.Initialize();
        this.collectionPanel.Initialize();
        this.messageBoxPanel.Initialize();
    }

    public void FadeIn(Action action = null)
    {
        StartCoroutine(FadeInCoroutine(action));
    }

    IEnumerator FadeInCoroutine(Action action)
    {
        this.fadeScreen.gameObject.SetActive(true);
        float alpha = 0;
        Image fade = this.fadeScreen;
        while (alpha < 1)
        {
            alpha += Time.deltaTime;
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, alpha);
            yield return null;
        }
        this.fadeScreen.gameObject.SetActive(false);
        action?.Invoke();
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine(this.fadeScreen));
    }

    IEnumerator FadeOutCoroutine(Image image)
    {
        image.gameObject.SetActive(true);
        float alpha = 1;
        Image fade = image;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime;
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, alpha);
            yield return null;
        }
        image.gameObject.SetActive(false);
    }

    public void ShowLoading()
    {
        this.loadingScreen.gameObject.SetActive(true);
    }

    public void HideLoading()
    {
        this.loadingText.gameObject.SetActive(false);
        StartCoroutine(FadeOutCoroutine(this.loadingScreen));
    }

    public void SetLoadingText(string text)
    {
        this.loadingText.text = text;
    }

    public static void ForceRebuild(params Text[] texts)
    {
        for (int i = 0; i < texts.Length; i++)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(texts[i].GetComponent<RectTransform>());
        }
    }
}
