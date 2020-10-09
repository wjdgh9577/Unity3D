using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillListContent : PoolObj
{
    public Image icon;
    public Text name;

    public void Refresh(int skillID)
    {
        icon.sprite = PreloadManager.Instance.preloadSprites[skillID];
        name.text = skillID.ToString();
    }
}
