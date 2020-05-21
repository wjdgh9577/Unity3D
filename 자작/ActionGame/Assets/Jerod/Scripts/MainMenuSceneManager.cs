using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuSceneManager : MonoBehaviour
{
    int state;    //기본(0), 게임시작(1), 크레딧(2)

    Rect rScrollRect;  // 화면상의 스크롤 뷰의 위치
    Rect rScrollArea; // 총 스크롤 되는 공간
    Vector2 vScrollPos; // 스크롤 바의 위치

    void Start()
    {
        this.state = 0;
        FadeManager.Instance.FadeOut();
    }

    private void OnGUI()
    {
        if (this.state == 0)  //초기화면
        {
            if (GUI.Button(new Rect(Screen.width / 2 + 250, Screen.height / 2 - 150, 200, 100), "게임 시작"))
            {
                this.state = 1;
            }
            else if (GUI.Button(new Rect(Screen.width / 2 + 250, Screen.height / 2, 200, 100), "크레딧"))
            {
                this.state = 2;
            }
            else if (GUI.Button(new Rect(Screen.width / 2 + 250, Screen.height / 2 + 150, 200, 100), "게임 종료"))
            {
                Quit();
            }
        }
        else if (this.state == 1)   //게임시작버튼 클릭 후 화면
        {
            if (GUI.Button(new Rect(Screen.width / 2 + 250, Screen.height / 2 - 100, 200, 100), "Name : \nPlay time : "))
            {
                this.state = 4;
                FadeManager.Instance.FadeIn("TutorialScene");
            }
            else if (GUI.Button(new Rect(Screen.width / 2 + 250, Screen.height / 2 + 100, 200, 100), "뒤로가기"))
            {
                this.state = 0;
            }
        }
        else if (this.state == 2)  //크레딧버튼 클릭 후 화면
        {
            this.rScrollRect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 200, 400, 400);
            this.rScrollArea = new Rect(0, 0, 500, 500);
            this.vScrollPos = GUI.BeginScrollView(this.rScrollRect, this.vScrollPos, this.rScrollArea);
            GUI.Label(new Rect(10, 10, 250, 30), "한양대학교 이정호");
            GUI.EndScrollView();
            
            if (GUI.Button(new Rect(Screen.width / 2 + 250, Screen.height / 2, 200, 100), "뒤로가기"))
            {
                this.state = 0;
            }
        }
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
