using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPScamera : MonoBehaviour
{
    public float rotateSpeed = 5f;

    public float cameraRotationLimit = 80f;

    float rotx, roty;
    float prex = 0;
    float prey = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        this.rotx = Input.GetAxis("Mouse X") * this.rotateSpeed;
        this.roty = Input.GetAxis("Mouse Y") * this.rotateSpeed;
        transform.localRotation *= Quaternion.Euler(this.prey, 0, 0);
        transform.localRotation *= Quaternion.Euler(0, -this.prex, 0);

        this.prex += this.rotx;
        this.prey = Mathf.Clamp(this.prey + this.roty, -this.cameraRotationLimit, this.cameraRotationLimit);

        transform.localRotation *= Quaternion.Euler(0, this.prex, 0);
        transform.localRotation *= Quaternion.Euler(-this.prey, 0, 0);
    }
}
