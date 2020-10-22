using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningScreen : MonoBehaviour
{
    [SerializeField]
    private Image screen;
    [SerializeField]
    private float speed = 0.2f;
    [SerializeField]
    private float threshold = 0.1f;
    private float alpha = 0.1f;
    private bool up = false;

    private void Update()
    {
        if (up)
        {
            alpha += Time.deltaTime * speed;
            screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, alpha);
            if (alpha >= threshold) up = false;
        }
        else
        {
            alpha -= Time.deltaTime * speed;
            screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, alpha);
            if (alpha <= 0) up = true;
        }
    }

    public void StartWarningScreen()
    {
        alpha = 0;
        up = true;
        Show();
        Invoke("Hide", 2);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
