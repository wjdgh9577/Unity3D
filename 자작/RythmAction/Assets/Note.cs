using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    GameObject gameDirector;

    // Start is called before the first frame update
    void Start()
    {
        this.gameDirector = GameObject.Find("GameDirector");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -0.5f)
        {
            Destroy(gameObject);
            this.gameDirector.GetComponent<GameDirector>().CallGameOverScene();
        }
        int FPS = Mathf.RoundToInt(1 / Time.deltaTime);
        float FPF = -Global.speed / FPS;
        transform.Translate(0, FPF, 0);

    }
}
