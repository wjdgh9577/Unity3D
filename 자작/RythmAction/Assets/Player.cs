using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    bool isHit;
    Collider note;
    public AudioClip bit;
    AudioSource aud;
    GameObject gameDirector;
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Renderer>().material.color = new Color(1.0f, 0, 0);
        this.isHit = false;
        this.aud = GetComponent<AudioSource>();
        this.gameDirector = GameObject.Find("GameDirector");
        gameObject.AddComponent<BoxCollider>().size = new Vector3(0.9f, Global.speed, 0.9f);
    }

    void SetScore()
    {
        float gap = Mathf.Abs(this.note.transform.position.y - transform.position.y);
        if (Global.auto) gap = 0;
        Global.score += 1000f / (gap + 1);
    }

    // Update is called once per frame
    void Update()
    {
        //입력에 관한 처리
        if (Global.auto)
        {
            if (this.isHit && this.note.transform.position.y <= 0)
            {
                //this.aud.PlayOneShot(this.bit);
                this.isHit = false;
                if (this.note.gameObject.tag == "upNote") transform.Translate(0, 0, 1);
                else if (this.note.gameObject.tag == "downNote") transform.Translate(0, 0, -1);
                else if (this.note.gameObject.tag == "leftNote") transform.Translate(-1, 0, 0);
                else if (this.note.gameObject.tag == "rightNote") transform.Translate(1, 0, 0);
                SetScore();
                Destroy(this.note.gameObject);
                this.note = null;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (!isHit || this.note.gameObject.tag != "upNote")
            {
                this.gameDirector.GetComponent<GameDirector>().CallGameOverScene();
                return;
            }
            if (transform.position.z < 8) transform.Translate(0, 0, 1);
            SetScore();
            //this.aud.PlayOneShot(this.bit);
            Destroy(this.note.gameObject);
            this.isHit = false;
            this.note = null;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (!isHit || this.note.gameObject.tag != "downNote")
            {
                this.gameDirector.GetComponent<GameDirector>().CallGameOverScene();
                return;
            }
            if (transform.position.z > 0) transform.Translate(0, 0, -1);
            SetScore();
            //this.aud.PlayOneShot(this.bit);
            Destroy(this.note.gameObject);
            this.isHit = false;
            this.note = null;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (!isHit || this.note.gameObject.tag != "leftNote")
            {
                this.gameDirector.GetComponent<GameDirector>().CallGameOverScene();
                return;
            }
            if (transform.position.x > 0) transform.Translate(-1, 0, 0);
            SetScore();
            //this.aud.PlayOneShot(this.bit);
            Destroy(this.note.gameObject);
            this.isHit = false;
            this.note = null;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (!isHit || this.note.gameObject.tag != "rightNote")
            {
                this.gameDirector.GetComponent<GameDirector>().CallGameOverScene();
                return;
            }
            if (transform.position.x < 8) transform.Translate(1, 0, 0);
            SetScore();
            //this.aud.PlayOneShot(this.bit);
            Destroy(this.note.gameObject);
            this.isHit = false;
            this.note = null;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isHit || this.note.gameObject.tag != "spaceNote")
            {
                this.gameDirector.GetComponent<GameDirector>().CallGameOverScene();
                return;
            }
            SetScore();
            //this.aud.PlayOneShot(this.bit);
            Destroy(this.note.gameObject);
            this.isHit = false;
            this.note = null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        this.isHit = true;
        this.note = other;
    }
}
