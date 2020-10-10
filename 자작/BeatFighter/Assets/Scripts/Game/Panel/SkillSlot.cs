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
        if (SkillMode.skillID == 0)
        {
            Debug.LogError("스킬 설명 출력 구현 필요");
        }
        else
        {
            PlayerData.currentSkills[index] = SkillMode.skillID;
            PlayerData.charDataDic[typeID].skills[index] = SkillMode.skillID;
            Refresh(typeID, SkillMode.skillID);
            SkillMode.skillID = 0;
        }
    }
}
