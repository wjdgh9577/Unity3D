using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkText : MonoBehaviour
{
    [SerializeField]
    private CustomText text;
    private float alpha = 1;
    private bool up = false;

    private void Update()
    {
        if (up)
        {
            alpha += Time.deltaTime;
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            if (alpha >= 1) up = false;
        }
        else
        {
            alpha -= Time.deltaTime;
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            if (alpha <= 0) up = true;
        }
    }
}
