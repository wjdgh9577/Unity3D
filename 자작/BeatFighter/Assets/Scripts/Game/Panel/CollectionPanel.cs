using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionPanel : PanelBase
{
    public void OnSkillButton()
    {
        Debug.LogError("스킬창 구현 필요!");
    }

    public void OnItemButton()
    {
        Debug.LogError("아이템창 구현 필요!");
    }

    public void OnBackButton()
    {
        GUIManager.Instance.menuPanel.Show();
        Hide();
    }
}
