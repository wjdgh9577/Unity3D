using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 오브젝트의 색상을 변경하는 스크립트
     */
public class BodyPaint : MonoBehaviour
{
    public Color color;

    void Awake()
    {
        GetComponent<Renderer>().material.color = this.color;
    }
}
