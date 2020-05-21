using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameoverText;
    public Text timeText;
    public Text recordText;

    private float surviveTime;
    private bool isGameover;

    void Start()
    {
        this.surviveTime = 0;
        this.isGameover = false;
    }

    void Update()
    {
        if (!this.isGameover)
        {
            this.surviveTime += Time.deltaTime;

            this.timeText.text = "Time : " + (int)this.surviveTime;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
    }

    public void EndGame()
    {
        this.isGameover = true;
        this.gameoverText.SetActive(true);

        float bestTime = PlayerPrefs.GetFloat("BestTime");

        if (this.surviveTime > bestTime)
        {
            bestTime = this.surviveTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }

        this.recordText.text = "Best Time : " + (int)bestTime;
    }
}
