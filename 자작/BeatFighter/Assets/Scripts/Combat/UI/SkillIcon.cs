using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillIcon : MonoBehaviour, IPointerDownHandler
{
    private PlayerChar player;
    private SkillInfo metaData;
    private float lastSkillTime = 0;

    private bool on = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        on = true;
    }

    void Update()
    {
        if (on)
        {
            if (Time.time - lastSkillTime > metaData.cooldown)
            {
                lastSkillTime = Time.time;
                JudgeRank judge = Combat.Instance.vitalSign.Judge();
                player.DoSkill(metaData.typeID, judge);
            }
            on = false;
        }
    }

    public void SetBaseData(PlayerChar player, SkillInfo meta)
    {
        this.player = player;
        this.metaData = meta;
    }
}
