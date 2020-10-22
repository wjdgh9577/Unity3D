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

    /// <summary>
    /// 스킬 선택창 활성화 또는 스킬 변경시 슬롯 초기화
    /// </summary>
    /// <param name="typeID"></param>
    /// <param name="skillID"></param>
    public void Refresh(int typeID, int skillID)
    {
        this.typeID = typeID;
        this.skillID = skillID;
        this.skillIcon.sprite = PreloadManager.Instance.TryGetSprite(skillID);
    }

    public void OnSelected()
    {
        if (GUIManager.Instance.collectionPanel.skillMode.skillID == 0)
        {
            GUIManager.Instance.collectionPanel.skillMode.SetDescription(skillID);
        }
        else
        {
            PlayerData.currentSkills[index] = GUIManager.Instance.collectionPanel.skillMode.skillID;
            PlayerData.charDataDic[typeID].currentSkills[index] = GUIManager.Instance.collectionPanel.skillMode.skillID;
            Refresh(typeID, GUIManager.Instance.collectionPanel.skillMode.skillID);
            GUIManager.Instance.collectionPanel.skillMode.skillID = 0;
            GUIManager.Instance.collectionPanel.skillMode.DeselectAll();
        }
    }
}
