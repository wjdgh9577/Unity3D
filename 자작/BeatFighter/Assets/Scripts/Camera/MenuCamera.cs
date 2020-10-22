using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    public float speed = 10;

    void LateUpdate()
    {
        transform.RotateAround(Vector3.zero, Vector3.up, Time.deltaTime * speed);
    }
}
