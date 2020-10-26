using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    private Text damageText;

    private int damage = 0;
    private float alpha = 0;

    private void Update()
    {
        if (this.alpha > 0)
        {
            this.damageText.color = new Color(this.damageText.color.r, this.damageText.color.g, this.damageText.color.b, this.alpha);
            this.alpha -= Time.deltaTime;
        }
        else gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        this.damage = 0;
    }

    public void Refresh(DamageInfo info)
    {
        if (this.damageText == null) this.damageText = GetComponent<Text>();

        this.alpha = 1;
        this.damage += info.damage;
        this.damageText.text = this.damage == 0 ? "Miss" : $"-{this.damage}";
        this.damageText.color = info.judge == JudgeRank.critical ? Color.yellow : Color.red;

        gameObject.SetActive(true);
    }

    public void Despawn()
    {
        this.alpha = 0;
        gameObject.SetActive(false);
    }
}
