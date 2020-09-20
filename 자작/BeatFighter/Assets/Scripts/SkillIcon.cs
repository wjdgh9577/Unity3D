﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillIcon : MonoBehaviour, IPointerDownHandler
{
    private PlayerChar player;
    private bool on = false;

    private void OnEnable()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerChar>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        on = true;
    }

    void Update()
    {
        if (on)
        {
            player.Attack();
            on = false;
        }
    }
}
