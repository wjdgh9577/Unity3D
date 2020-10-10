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

    public static int skillID;

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
    }

    public void OnBackButton()
    {
        Hide();
    }

    public static void SetSelectedSkill(int skillID)
    {
        SkillMode.skillID = skillID;
        Debug.LogError("스킬 설명 출력 구현 필요");
    }
}
