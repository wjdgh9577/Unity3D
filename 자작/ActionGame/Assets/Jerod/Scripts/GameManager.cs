using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerInput playerInput;  //플레이어의 Input
    public bool cursorLockOn = true;  //커서 락상태
    public int frame = 60;  //프레임
    public float playTime;  //플레이타임

    private void Awake()
    {
        Application.targetFrameRate = frame;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (playerInput.ctrlButtonDown)
        {
            SetCursorLock();
        }
    }

    private void SetCursorLock()
    {
        if (cursorLockOn)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorLockOn = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cursorLockOn = true;
        }
    }
}
