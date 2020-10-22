using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillListContent : PoolObj
{
    public Image skillIcon;
    public Text skillName;

    private int typeID;
    private int skillID;
    private Image image;

    /// <summary>
    /// 스킬 리스트 생성시 초기화
    /// </summary>
    /// <param name="typeID"></param>
    /// <param name="skillID"></param>
    public void Refresh(int typeID, int skillID)
    {
        if (image == null) image = GetComponent<Image>();

        this.typeID = typeID;
        this.skillID = skillID;
        this.skillIcon.sprite = PreloadManager.Instance.TryGetSprite(skillID);
        this.skillName.text = string.Format("Lv.{0} {1}", PlayerData.GetSkillData(skillID).level, TableData.instance.GetString(skillID.ToString()));

        Deselected();
    }

    public void Deselected()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, (float)100 / 255);
    }

    public void OnSelected()
    {
        GUIManager.Instance.collectionPanel.skillMode.DeselectAll();
        GUIManager.Instance.collectionPanel.skillMode.skillID = skillID;
        GUIManager.Instance.collectionPanel.skillMode.SetDescription(skillID);
        image.color = new Color(image.color.r, image.color.g, image.color.b, (float)150 / 255);
    }
}
