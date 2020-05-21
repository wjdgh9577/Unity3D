using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 8f;

    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float xSpeed = Input.GetAxis("Horizontal") * this.speed;
        float zSpeed = Input.GetAxis("Vertical") * this.speed;

        Vector3 newVelocity = new Vector3(xSpeed, 0, zSpeed);
        this.rb.velocity = newVelocity;
    }

    public void Die()
    {
        gameObject.SetActive(false);

        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.EndGame();
    }
}
