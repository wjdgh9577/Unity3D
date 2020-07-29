using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    private Rigidbody rigidbody;

    [SerializeField]
    private float jumpPower;

    private bool canJump = true;
    private bool canFly = false;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Jump()
    {
        if (canJump)
        {
            rigidbody.velocity = Vector3.up * jumpPower;
            canJump = false;
        }
    }

    public void Dive()
    {
        rigidbody.velocity = Vector3.down * jumpPower * 10;
    }

    public void Fly()
    {
        if (canFly)
        {
            rigidbody.velocity = Vector3.down * .1f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Field" && collision.contacts[0].normal.y == 1)
        {
            canJump = true;
            canFly = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Field")
        {
            canFly = true;
        }
    }
}
