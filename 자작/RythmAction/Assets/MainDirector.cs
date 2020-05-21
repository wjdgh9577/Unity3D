using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainDirector : MonoBehaviour
{
    GameObject gameStart;
    GameObject exit;
    GameObject cursor;
    int status;

    // Start is called before the first frame update
    void Start()
    {
        this.cursor = GameObject.Find("Cursor");
        this.gameStart = GameObject.Find("GameStart");
        this.exit = GameObject.Find("Exit");
        this.status = 0;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (this.status < 1 && this.status >= 0)
            {
                this.status += 1;
                this.cursor.transform.Translate(0, -140, 0);
            }
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (this.status > 0 && this.status <= 1)
            {
                this.status -= 1;
                this.cursor.transform.Translate(0, 140, 0);
            }
        }
        else if(Input.GetKeyDown(KeyCode.Return))
        {
            if(this.status == 0)
            {
                SceneManager.LoadScene("MenuScene");
            }
            else if(this.status == 1)
            {
                Quit();
            }
        }
    }
}
