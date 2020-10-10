using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public Image skillIcon;

    private int typeID;
    private int skillID;
    [SerializeField]
    private int index;

    public void Refresh(int typeID, int skillID)
    {
        this.typeID = typeID;
        this.skillID = skillID;
        this.skillIcon.sprite = PreloadManager.Instance.preloadSprites[skillID];
    }

    public void OnSelected()
    {
        if (GUIManager.Instance.skillMode.skillID == 0)
        {
            GUIManager.Instance.skillMode.SetDescription(skillID);
        }
        else
        {
            PlayerData.currentSkills[index] = GUIManager.Instance.skillMode.skillID;
            PlayerData.charDataDic[typeID].skills[index] = GUIManager.Instance.skillMode.skillID;
            Refresh(typeID, GUIManager.Instance.skillMode.skillID);
            GUIManager.Instance.skillMode.skillID = 0;
        }
    }
}
