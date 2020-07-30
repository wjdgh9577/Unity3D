using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    private Rigidbody rigidbody;
    private BoxCollider boxCollider;

    [SerializeField]
    private float jumpPower;

    private bool canJump = true;
    private bool canFly = false;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
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
        Vector3 extents = boxCollider.bounds.extents;
        Vector3 center = boxCollider.bounds.center;
        Vector3 frontBottom = new Vector3(center.x + extents.x, center.y - extents.y, center.z);
        Vector3 backBottom = new Vector3(center.x - extents.x, center.y - extents.y, center.z);

        if (Physics.Raycast(frontBottom, Vector3.down, out RaycastHit hit) || Physics.Raycast(backBottom, Vector3.down, out hit))
        {
            if (hit.transform.CompareTag("Field"))
            {
                rigidbody.position += Vector3.down * hit.distance;
                rigidbody.velocity = Vector3.zero;
            }
        }
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
        if (collision.transform.CompareTag("Field") && collision.contacts[0].normal.y == 1)
        {
            canJump = true;
            canFly = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Field"))
        {
            canFly = true;
        }
    }
}
