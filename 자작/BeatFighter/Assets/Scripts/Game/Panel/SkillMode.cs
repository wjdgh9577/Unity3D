using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMode : PanelBase
{
    [Header("Skill List")]
    public Transform contentsTM;
    public GameObject prefeb;

    [Header("Skill Slots")]
    public SkillSlot[] slots;

    [System.NonSerialized]
    public List<SkillListContent> contents;

    public int skillID;

    /// <summary>
    /// 스킬 선택창 활성화시 초기화
    /// </summary>
    /// <param name="typeID"></param>
    public void Refresh(int typeID)
    {
        if (contents == null) contents = new List<SkillListContent>();
        
        foreach (var content in contents) content.Despawn();
        contents.Clear();

        for (int i = 0; i < PlayerData.charDataDic[typeID].skills.Count; i++)
        {
            int skill = PlayerData.charDataDic[typeID].skills[i];
            PlayerData.currentSkills[i] = skill;
            slots[i].Refresh(typeID, skill);
        }
        foreach (int skill in TableData.instance.skillSetDataDic[typeID].skillIDs)
        {
            SkillListContent content = PoolingManager.Instance.Spawn<SkillListContent>(prefeb, contentsTM);
            content.Refresh(typeID, skill);
            contents.Add(content);
        }

        skillID = 0;
    }

    /// <summary>
    /// 스킬 리스트의 모든 스킬의 선택상태를 취소
    /// </summary>
    public void DeselectAll()
    {
        foreach (var content in contents)
        {
            content.Deselected();
        }
    }

    /// <summary>
    /// 선택된 스킬의 설명 출력
    /// </summary>
    /// <param name="skillID"></param>
    public void SetDescription(int skillID)
    {
        Debug.LogError("스킬 설명 출력 구현 필요");
    }

    public void OnBackButton()
    {
        Hide();
    }
}
