using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    GameObject titleText;
    GameObject artistText;
    GameObject scoreText;
    GameObject remaintimeText;
    public AudioClip[] clips;
    AudioSource aud;
    float time;
    bool isPlay;

    void setClip()
    {
        foreach(AudioClip clip in this.clips)
        {
            if (clip.name.Equals(Global.title)) this.aud.clip = clip;
        }
    }
    void playClip(bool isPlay)
    {
        if (!isPlay) this.aud.Play();
        this.isPlay = true;
    }

    public void CallGameOverScene()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    public void CallGameClearScene()
    {
        SceneManager.LoadScene("GameClearScene");
    }

    // Start is called before the first frame update
    void Start()
    {
        this.time = 0;
        this.isPlay = false;
        this.titleText = GameObject.Find("Title");
        this.artistText = GameObject.Find("Artist");
        this.scoreText = GameObject.Find("Score");
        this.remaintimeText = GameObject.Find("Remaintime");
        this.aud = GetComponent<AudioSource>();

        Application.targetFrameRate = Global.FPS;
        setClip();
    }

    // Update is called once per frame
    void Update()
    {
        this.time += Time.deltaTime;

        //게임 시작 1초 후 클립 시작
        if (this.time > 1) playClip(this.isPlay);

        //ESC로 메뉴 복귀
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Global.score = 0;
            SceneManager.LoadScene("MenuScene");
        }

        //UI 출력
        this.titleText.GetComponent<Text>().text = "Title : " + Global.title;
        this.artistText.GetComponent<Text>().text = "Artist : " + Global.artist;
        this.scoreText.GetComponent<Text>().text = "Score : " + Global.score.ToString();
        this.remaintimeText.GetComponent<Text>().text = "Time : " + (Global.playtime - this.time).ToString("F1");

        //클리어 판정
        if (Global.playtime < this.time) CallGameClearScene();
    }
}
