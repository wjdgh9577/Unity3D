using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboText : MonoBehaviour
{
    private Text comboText;

    private float alpha = 0;

    private void Update()
    {
        if (alpha > 0)
        {
            comboText.color = new Color(comboText.color.r, comboText.color.g, comboText.color.b, alpha);
            alpha -= Time.deltaTime;
        }
        else gameObject.SetActive(false);
    }

    public void Show(int combo)
    {
        if (comboText == null) comboText = GetComponent<Text>();
        alpha = 1;

        if (combo < 11)
        { 
            comboText.text = combo + " Combo!";
            comboText.color = new Color(1, (11 - combo) * 0.1f, (11 - combo) * 0.1f);
        }
        else
        {
            comboText.text = combo + " Max Combo!";
            comboText.color = Color.red;
        }

        gameObject.SetActive(true);
    }
}
