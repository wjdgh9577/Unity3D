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

    public List<SkillListContent> contents;

    public void Refresh(int typeID)
    {
        if (contents == null) contents = new List<SkillListContent>();
        
        foreach (var content in contents) content.Despawn();
        contents.Clear();

        for (int i = 0; i < PlayerData.charDataDic[typeID].skills.Count; i++)
        {
            int skill = PlayerData.charDataDic[typeID].skills[i];
            PlayerData.currentSkills[i] = skill;
            slots[i].Refresh(skill);
        }
        foreach (int skill in TableData.instance.skillSetDataDic[typeID].skillIDs)
        {
            SkillListContent content = PoolingManager.Instance.Spawn<SkillListContent>(prefeb, contentsTM);
            content.Refresh(skill);
            contents.Add(content);
        }
    }

    public void OnBackButton()
    {
        Hide();
    }
}
