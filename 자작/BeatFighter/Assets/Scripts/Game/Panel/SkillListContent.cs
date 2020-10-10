using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillListContent : PoolObj
{
    public Image icon;
    public Text name;

    private int typeID;
    private int skillID;

    public void Refresh(int typeID, int skillID)
    {
        this.typeID = typeID;
        this.skillID = skillID;
        this.icon.sprite = PreloadManager.Instance.preloadSprites[skillID];
        this.name.text = skillID.ToString();
    }

    public void OnSelected()
    {
        SkillMode.SetSelectedSkill(skillID);
    }
}
