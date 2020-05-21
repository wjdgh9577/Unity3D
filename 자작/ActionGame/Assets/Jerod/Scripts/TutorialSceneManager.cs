using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSceneManager : MonoBehaviour
{
    public GUISkin mySkin;
    public GameObject[] dummys;
    public GameObject wall;
    public GameObject portal;

    IEnumerator stage;

    void Awake()
    {
        FadeManager.Instance.FadeOut();
        this.dummys[1].SetActive(false);
        this.dummys[2].SetActive(false);
        this.stage = Stage();
        StartCoroutine(this.stage);
    }

    private IEnumerator Stage()
    {
        while (true)
        {
            yield return null;

            if (this.dummys[0] == null)
            {
                this.dummys[1].SetActive(true);
                this.dummys[2].SetActive(true);
                yield return new WaitUntil(() => this.dummys[1] == null && this.dummys[2] == null);
                this.wall.SetActive(false);
                StopCoroutine(this.stage);
            }
        }
    }

    private void OnGUI()
    {
        GUI.skin = this.mySkin;
        GUI.Box(new Rect(Screen.width - 200, 0, 200, 180), "조작법");
        GUI.Label(new Rect(Screen.width - 190, 30, 200, 200), "W, A, S, D : 이동\n스페이스바 : 점프\n좌클릭 : 잽\n우클릭 : 하이킥\n이동+좌클릭 : 라이징 펀치\n이동+우클릭 : 스핀킥\nAlt : 스크류킥\nCtrl : 커서 보이기");
    }

    public void Portal(string nextScene)
    {
        FadeManager.Instance.FadeIn(nextScene);
    }
}
