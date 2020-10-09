using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public Image skillIcon;

    private int skillID;

    public void Refresh(int skillID)
    {
        this.skillID = skillID;
        this.skillIcon.sprite = PreloadManager.Instance.preloadSprites[skillID];
    }
}
