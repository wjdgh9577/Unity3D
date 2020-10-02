using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : Singleton<FadeManager>
{
    [SerializeField]
    private Image fadeScreen;

    public void FadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }

    IEnumerator FadeInCoroutine()
    {
        fadeScreen.gameObject.SetActive(true);
        float alpha = 0;
        Image fade = fadeScreen;
        fade.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        yield return null;
        while (alpha < 1)
        {
            alpha += Time.deltaTime;
            fade.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    IEnumerator FadeOutCoroutine()
    {
        fadeScreen.gameObject.SetActive(true);
        float alpha = 1;
        Image fade = fadeScreen;
        fade.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        yield return null;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime;
            fade.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }
}
