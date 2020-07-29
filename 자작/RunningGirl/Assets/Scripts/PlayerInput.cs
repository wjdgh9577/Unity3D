using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerPhysics playerPhysics;

    void Awake()
    {
        playerPhysics = GetComponent<PlayerPhysics>();
    }

    void Start()
    {
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
        Vector2 startPos = Vector2.zero;
        Vector2 currentPos = Vector2.zero;
        bool enableJump = true;
        bool enableDive = true;
        
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPos = Input.mousePosition;
                currentPos = startPos;
            }
            else if (Input.GetMouseButton(0))
            {
                currentPos = Input.mousePosition;
                if (currentPos.y - startPos.y > 100)
                {
                    if (enableJump)
                    {
                        playerPhysics.Jump();
                        enableJump = false;
                    }
                }
                else if (currentPos.y - startPos.y < -100)
                {
                    if (enableDive)
                    {
                        playerPhysics.Dive();
                        enableDive = false;
                    }
                }
                else
                {
                    playerPhysics.Fly();
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                startPos = Input.mousePosition;
                currentPos = startPos;
                enableJump = true;
                enableDive = true;
            }
            yield return null;
        }
    }
}
