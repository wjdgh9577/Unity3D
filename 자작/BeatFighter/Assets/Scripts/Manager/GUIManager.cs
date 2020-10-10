using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : Singleton<GUIManager>
{
    public LoginPanel loginPanel;
    public MenuPanel menuPanel;
    public CollectionPanel collectionPanel;
    public SkillMode skillMode;

    [SerializeField]
    private Image fadeScreen;
    [SerializeField]
    private Image loadingScreen;
    [SerializeField]
    private Text loadingText;

    public void FadeIn(Action action)
    {
        StartCoroutine(FadeInCoroutine(action));
    }

    IEnumerator FadeInCoroutine(Action action)
    {
        fadeScreen.gameObject.SetActive(true);
        float alpha = 0;
        Image fade = fadeScreen;
        while (alpha < 1)
        {
            alpha += Time.deltaTime;
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, alpha);
            yield return null;
        }
        fadeScreen.gameObject.SetActive(false);
        action();
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine(fadeScreen));
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
        loadingScreen.gameObject.SetActive(true);
    }

    public void HideLoading()
    {
        loadingText.gameObject.SetActive(false);
        StartCoroutine(FadeOutCoroutine(loadingScreen));
    }

    public void SetLoadingText(string text)
    {
        loadingText.text = text;
    }
}
