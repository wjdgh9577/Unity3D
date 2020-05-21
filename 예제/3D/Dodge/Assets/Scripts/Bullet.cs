using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 8f;
    private Rigidbody rb;

    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
        this.rb.velocity = transform.forward * this.speed;

        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController player = other.GetComponentInParent<PlayerController>();

            if (player != null)
            {
                player.Die();
            }
        }
    }
}
