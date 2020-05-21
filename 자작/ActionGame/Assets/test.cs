using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 movement;
    private float x;
    private float jumpP = 5;
    private bool jump;

    // Start is called before the first frame update
    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) jump = true;

        x = Input.GetAxis("Horizontal") * 5;
    }

    private void FixedUpdate()
    {
        if (jump)
        {
            jump = false;
            //this.rb.velocity = new Vector3(this.rb.velocity.x, 0, this.rb.velocity.z);
            this.rb.AddForce(Vector3.up * jumpP, ForceMode.VelocityChange);
        }

        if (this.tag == "1")
        {
            this.rb.velocity = new Vector3(0, this.rb.velocity.y, 0);
            this.rb.AddForce(Vector3.forward * x, ForceMode.VelocityChange);
        }
        else
        {
            this.rb.velocity = new Vector3(0, this.rb.velocity.y, x);
        }
    }
}
