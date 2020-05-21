using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuDirector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //키입력 관련
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("GameScene");
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (Global.speed < 30) Global.speed++;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Global.speed > 1) Global.speed--;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {

        }
        else if (Input.GetKeyDown(KeyCode.F12)) Global.auto = !Global.auto;
        else if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene("MainScene");

        //UI
        if(Global.auto)
        {
            GameObject.Find("Auto").GetComponent<Text>().text = "Auto";
        }
        else
        {
            GameObject.Find("Auto").GetComponent<Text>().text = "";
        }
    }
}
