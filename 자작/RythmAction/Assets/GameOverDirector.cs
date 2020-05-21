using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverDirector : MonoBehaviour
{
    GameObject scoreText;

    // Start is called before the first frame update
    void Start()
    {
        this.scoreText = GameObject.Find("Score");
    }

    // Update is called once per frame
    void Update()
    {
        this.scoreText.GetComponent<Text>().text = "Score : " + Global.score.ToString("F0");
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Global.score = 0;
            SceneManager.LoadScene("MenuScene");
        }
    }
}
