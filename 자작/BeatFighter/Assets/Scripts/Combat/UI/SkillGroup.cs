using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGroup : MonoBehaviour
{
    [SerializeField]
    private SkillIcon[] skillIcons;
    private PlayerChar player;

    private void Awake()
    {
        Combat.onMapSet += SetBaseData;
        Combat.onStageSet += Show;
        Combat.onStageEnd += Hide;
        PlayerChar.onSkillPrepared += LockSkillIcons;
        PlayerChar.onSkillInitialized += UnlockSkillIcons;
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 맵이 생성될 때 스킬아이콘을 초기화한다. 
    /// </summary>
    public void SetBaseData()
    {
        player = Combat.Instance.player;
        for (int i = 0; i < skillIcons.Length; i++)
        {
            SkillInfo info = TableData.instance.skillDataDic[PlayerData.currentSkills[i]];
            skillIcons[i].SetBaseData(player, info);
        }
    }

    public void LockSkillIcons()
    {
        for (int i = 0; i < skillIcons.Length; i++)
        {
            skillIcons[i].Lock();
        }
    }

    public void UnlockSkillIcons()
    {
        for (int i = 0; i < skillIcons.Length; i++)
        {
            skillIcons[i].Unlock();
        }
    }
}
