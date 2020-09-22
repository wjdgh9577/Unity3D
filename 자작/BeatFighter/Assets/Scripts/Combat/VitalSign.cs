using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitalSign : MonoBehaviour
{
    public RectTransform heartTM;
    public RectTransform poolLineTM;
    public GameObject note;

    private void Awake()
    {
        Combat.onStageSet += Show;
        Combat.onStageEnd += Hide;
        Hide();
    }

    void Start()
    {
        PoolNote();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 노트 생성
    /// </summary>
    public void PoolNote()
    {
        GameObject newNote = Instantiate(note, poolLineTM.position, Quaternion.identity, heartTM) as GameObject;
    }
}
