using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillListContent : PoolObj
{
    public Image skillIcon;
    public Text skillName;
    public GameObject lockObj;
    public Text lockText;

    private int typeID;
    private int skillID;
    private int unlockLevel;
    private Image image;

    /// <summary>
    /// 스킬 리스트 생성시 초기화
    /// </summary>
    /// <param name="typeID"></param>
    /// <param name="skillID"></param>
    public void Refresh(int typeID, int skillID)
    {
        if (this.image == null) this.image = GetComponent<Image>();

        this.typeID = typeID;
        this.skillID = skillID;
        this.unlockLevel = TableData.instance.skillDataDic[skillID].unlockLevel;
        this.skillIcon.sprite = PreloadManager.Instance.TryGetSprite(skillID);
        this.skillName.text = string.Format("Lv.{0} {1}", PlayerData.GetSkillData(skillID).level, TableData.instance.GetString("Skill_" + skillID.ToString()));

        this.lockText.text = string.Format(TableData.instance.GetString("Skill_Lock"), this.unlockLevel);
        this.lockObj.SetActive(PlayerData.charDataDic[typeID].level < this.unlockLevel);

        Deselected();
    }

    public void Deselected()
    {
        this.image.color = new Color(this.image.color.r, this.image.color.g, this.image.color.b, (float)100 / 255);
    }

    public void OnSelected()
    {
        GUIManager.Instance.collectionPanel.skillMode.DeselectAll();
        GUIManager.Instance.collectionPanel.skillMode.skillID = PlayerData.charDataDic[typeID].level < this.unlockLevel ? 0 : skillID;
        GUIManager.Instance.collectionPanel.skillMode.SetDescription(skillID);
        this.image.color = new Color(this.image.color.r, this.image.color.g, this.image.color.b, (float)150 / 255);
    }
}
