using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitalSign : MonoBehaviour
{
    public RectTransform heartTM;
    public RectTransform poolLineTM;
    public GameObject note;

    void Start()
    {
        PoolNote();
    }

    public void PoolNote()
    {
        GameObject newNote = Instantiate(note, poolLineTM.position, Quaternion.identity, heartTM) as GameObject;
    }
}
