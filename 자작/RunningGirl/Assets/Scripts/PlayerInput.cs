using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerPhysics playerPhysics;

    private Vector2 pos;

    void Start()
    {
        playerPhysics = GetComponent<PlayerPhysics>();
        StartCoroutine(Drag());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            playerPhysics.Jump();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            playerPhysics.Dive();
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            playerPhysics.Fly();
        }
    }

    IEnumerator Drag()
    {
        while (true)
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 nextPos = Input.mousePosition;
                if (nextPos.y - pos.y > 10)
                {
                    playerPhysics.Jump();
                }
                pos = nextPos;
                Debug.Log(pos);
            }
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}
