using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance = null;

    public GameObject fadeInOut;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void FadeIn(string nextScene)
    {
        StartCoroutine(FadeInCoroutine(nextScene));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    IEnumerator FadeInCoroutine(string nextScene)
    {
        float alpha = 0;
        Image fade = this.fadeInOut.GetComponent<Image>();
        fade.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        yield return null;
        while(alpha < 1)
        {
            alpha += 0.02f;
            fade.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        LoadingSceneManager.LoadScene(nextScene);
    }

    IEnumerator FadeOutCoroutine()
    {
        float alpha = 1;
        Image fade = this.fadeInOut.GetComponent<Image>();
        fade.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        yield return null;
        while (alpha > 0)
        {
            alpha -= 0.02f;
            fade.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }
}
