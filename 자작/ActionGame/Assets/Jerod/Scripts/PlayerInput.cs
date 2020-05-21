using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //이동 관련
    public float horizontal { get; private set; }
    public float vertical { get; private set; }
    public bool space { get; private set; }
    public float shift { get; private set; }
    public float verticalMouse { get; private set; }
    public float horizontalMouse { get; private set; }
    public float scroll { get; private set; }
    public bool scrollButtonDown { get; private set; }
    public bool scrollButtonUp { get; private set; }
    public bool scrollButton { get; private set; }

    //공격 관련
    public bool leftMouseButton { get; private set; }
    public bool rightMouseButton { get; private set; }
    public bool tabButtonDown { get; private set; }

    //시스템 관련
    public bool ctrlButtonDown { get; private set; }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        space = Input.GetKeyDown(KeyCode.Space);
        shift = Input.GetAxis("Fire3");
        verticalMouse = Input.GetAxis("Mouse Y");
        horizontalMouse = Input.GetAxis("Mouse X");
        scroll = Input.GetAxis("Mouse ScrollWheel");
        scrollButtonDown = Input.GetMouseButtonDown(2);
        scrollButtonUp = Input.GetMouseButtonUp(2);
        scrollButton = Input.GetMouseButton(2);

        leftMouseButton = Input.GetMouseButton(0);
        rightMouseButton = Input.GetMouseButton(1);
        tabButtonDown = Input.GetKeyDown(KeyCode.Tab);

        ctrlButtonDown = Input.GetKeyDown(KeyCode.LeftControl);
    }
}
