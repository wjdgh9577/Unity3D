using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour, IPointerDownHandler
{
    public GameObject _lock;
    public Text _text;

    private PlayerChar player;
    private SkillInfo metaData;
    private float lastSkillTime = float.NegativeInfinity;
    private float remainTime { get { return Mathf.Clamp(metaData.cooldown + lastSkillTime - Time.time, 0, metaData.cooldown); } }
    private bool isCooldown { get { return Time.time - lastSkillTime < metaData.cooldown; } }

    private bool on = false;
    private bool isLock = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        on = true;
    }

    void Update()
    {
        if (on && !isLock && !isCooldown)
        {
            lastSkillTime = Time.time;
            _text.gameObject.SetActive(true);
            JudgeRank judge = Combat.Instance.vitalSign.Judge();Debug.Log(judge);
            player.DoSkill(metaData.typeID, judge);
        }
        if (isCooldown)
        {
            _text.text = ((int)remainTime).ToString();
        }
        else
        {
            _text.gameObject.SetActive(false);
            _lock.SetActive(false);
        }
        if (isLock) _lock.SetActive(true);
        else if (!isLock && !isCooldown) _lock.SetActive(false);
        
        on = false;
    }

    public void SetBaseData(PlayerChar player, SkillInfo meta)
    {
        this.player = player;
        this.metaData = meta;
    }

    public void Lock()
    {
        isLock = true;
    }

    public void Unlock()
    {
        isLock = false;
    }
}
