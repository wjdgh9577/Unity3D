using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicTable : MonoBehaviour
{
    GameObject speedText;
    int musicNumber;

    // Start is called before the first frame update
    void Start()
    {
        this.speedText = transform.Find("Speed").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        this.speedText.GetComponent<TextMeshPro>().text = "Speed : " + Global.speed.ToString();
    }
}
