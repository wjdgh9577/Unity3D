using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    GameObject hpGauge;
    GameObject GameOver;

    // Start is called before the first frame update
    void Start()
    {
        this.hpGauge = GameObject.Find("hpGauge");
        this.GameOver = GameObject.Find("GameOver");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DecreaseHp()
    {
        this.hpGauge.GetComponent<Image>().fillAmount -= 0.1f;
        if (this.hpGauge.GetComponent<Image>().fillAmount == 0)
        {
            this.GameOver.GetComponent<Text>().text = "Game Over!";
        }
    }
}
