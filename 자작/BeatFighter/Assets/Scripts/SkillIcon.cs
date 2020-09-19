using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public struct DamageInfo
{
    BaseChar from;
    BaseChar to;
    int damage;
}

public class SkillIcon : MonoBehaviour, IPointerDownHandler
{
    private PlayerChar player;
    private bool on = false;

    private void Awake()
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
