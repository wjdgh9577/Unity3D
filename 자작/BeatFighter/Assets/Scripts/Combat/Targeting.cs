using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed;

    void FixedUpdate()
    {
        transform.Rotate(Vector3.forward, rotationSpeed);
    }
}
