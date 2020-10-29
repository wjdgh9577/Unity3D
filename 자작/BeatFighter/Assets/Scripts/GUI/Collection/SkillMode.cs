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
    public Text skillCooldown;
    public Text skillDescription;
    public Text skillExp;

    [System.NonSerialized]
    public List<SkillListContent> contents;
    [System.NonSerialized]
    public int skillID;

    public override void Initialize() { }

    public override void Show()
    {
        base.Show();
        if (PlayerData.tutorial_skillmode == 0)
        {
            GUIManager.Instance.messageBoxPanel.CallOKMessageBox("Message_Tutorial_SkillMode", () => { PlayerData.tutorial_skillmode = 1; PlayerData.SaveData(); });
        }
    }

    /// <summary>
    /// 스킬 선택창 활성화시 초기화
    /// </summary>
    /// <param name="typeID"></param>
    protected override void Refresh()
    {
        int typeID = PlayerData.currentChar;
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
        SkillInfo info = TableData.instance.skillDataDic[skillID];
        PlayerData.SkillData skillData = PlayerData.GetSkillData(skillID);
        int exp = skillData.exp;
        int requireExp = TableData.instance.skillExpDataDic[skillData.level].requireExp;
        object[] args = new object[] { Formula.CalcAtkMul(info.atkMul, info.atkMulPerLevel, skillData.level) * 100, Formula.CalcAtkAdd(info.atkAdd, info.atkAddPerLevel, skillData.level) };

        this.skillIcon.sprite = PreloadManager.Instance.TryGetSprite(skillID);
        this.skillName.text = TableData.instance.GetString("Skill_" + skillID.ToString());
        this.skillCooldown.text = string.Format(TableData.instance.GetString("Description_SkillCooldown"), info.cooldown);
        this.skillDescription.text = string.Format(TableData.instance.GetString("Description_" + skillID), args);
        this.skillExp.text = string.Format(TableData.instance.GetString("Description_SkillExp"), exp, requireExp);

        GUIManager.ForceRebuild(this.skillDescription);
    }
}
