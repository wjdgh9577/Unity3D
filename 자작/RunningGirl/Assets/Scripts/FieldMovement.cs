using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldMovement : MonoBehaviour
{
    private float speed = 0.1f;

    void FixedUpdate()
    {
        transform.position += Vector3.left * speed;
    }

    public void SetPosition(float x, float y, float z = 0)
    {
        transform.position = new Vector3(x, y, z);
    }

    public void SetSpeed(float _speed)
    {
        speed = _speed;
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
