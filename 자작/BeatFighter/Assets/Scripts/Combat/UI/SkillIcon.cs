using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillIcon : MonoBehaviour, IPointerDownHandler
{
    private PlayerChar player;
    private bool on = false;

    private void OnEnable()
    {
        player = Combat.Instance.player;
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
            Combat.Instance.vitalSign.Judge();
            on = false;
        }
    }
}
