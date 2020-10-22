using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGroup : MonoBehaviour
{
    [SerializeField]
    private SkillIcon[] skillIcons;
    private PlayerChar player;

    public void Initialize()
    {
        CombatManager.onMapSet += Refresh;
        CombatManager.onMapSet += LockSkillIcons;
        CombatManager.onMapEnd += Hide;
        CombatManager.onStageSet += Show;
        CombatManager.onStageStart += UnlockSkillIcons;
        CombatManager.onStageEnd += LockSkillIcons;
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

    public void Refresh()
    {
        for (int i = 0; i < this.skillIcons.Length; i++)
        {
            this.skillIcons[i].Refresh();
        }
    }

    /// <summary>
    /// 맵이 생성될 때 스킬아이콘을 초기화한다.
    /// </summary>
    /// <param name="player"></param>
    public void SetBaseData(PlayerChar player)
    {
        this.player = player;
        for (int i = 0; i < this.skillIcons.Length; i++)
        {
            SkillInfo info = TableData.instance.skillDataDic[PlayerData.currentSkills[i]];
            this.skillIcons[i].SetBaseData(player, info);
        }
    }

    public void LockSkillIcons()
    {
        for (int i = 0; i < this.skillIcons.Length; i++)
        {
            this.skillIcons[i].Lock();
        }
    }

    public void UnlockSkillIcons()
    {
        if (CombatManager.Instance.mobCount == 0) return;
        for (int i = 0; i < this.skillIcons.Length; i++)
        {
            this.skillIcons[i].Unlock();
        }
    }
}
