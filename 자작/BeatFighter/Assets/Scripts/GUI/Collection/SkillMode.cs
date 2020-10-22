using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillMode : PanelBase
{
    [Header("Skill List")]
    public Transform contentsTM;
    public GameObject prefeb;

    [Header("Skill Slots")]
    public SkillSlot[] slots;

    [Header("Skill Description")]
    public Image skillIcon;
    public Text skillName;
    public Text skillDescription;
    public Text skillExp;

    [System.NonSerialized]
    public List<SkillListContent> contents;
    [System.NonSerialized]
    public int skillID;

    public override void Initialize()
    {
        
    }

    /// <summary>
    /// 스킬 선택창 활성화시 초기화
    /// </summary>
    /// <param name="typeID"></param>
    public void Refresh(int typeID)
    {
        if (contents == null) contents = new List<SkillListContent>();
        
        foreach (var content in contents) content.Despawn();
        contents.Clear();

        for (int i = 0; i < PlayerData.charDataDic[typeID].currentSkills.Count; i++)
        {
            int skill = PlayerData.charDataDic[typeID].currentSkills[i];
            slots[i].Refresh(typeID, skill);
        }
        foreach (int skill in TableData.instance.skillSetDataDic[typeID].skillIDs)
        {
            SkillListContent content = PoolingManager.Instance.Spawn<SkillListContent>(prefeb, contentsTM);
            content.Refresh(typeID, skill);
            contents.Add(content);
        }
        SetDescription(PlayerData.currentSkills[0]);
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
        skillIcon.sprite = PreloadManager.Instance.TryGetSprite(skillID);
        skillName.text = TableData.instance.GetString(skillID.ToString());
        skillDescription.text = TableData.instance.GetString("Description_" + skillID);
        PlayerData.SkillData skillData = PlayerData.GetSkillData(skillID);
        int exp = skillData.exp;
        int requireExp = TableData.instance.skillExpDataDic[skillData.level].requireExp;
        skillExp.text = string.Format("{0} / {1}", exp, requireExp);
    }
}
