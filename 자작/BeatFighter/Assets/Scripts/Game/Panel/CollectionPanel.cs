using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionPanel : PanelBase
{
    public SkillMode skillMode;

    private Dictionary<int, PlayerData.CharData> charDic;
    private List<int> charList;
    private int index;

    protected override void Refresh()
    {
        charDic = PlayerData.charDataDic;
        if (charList == null) charList = new List<int>();
        charList.Clear();
        foreach (var key in charDic.Keys)
        {
            charList.Add(key);
        }
        charList.Sort();
        index = charList.IndexOf(PlayerData.currentChar);
        skillMode.Refresh(charList[index]);
    }

    public void OnSkillButton()
    {
        skillMode.Show();
    }

    public void OnItemButton()
    {
        Debug.LogError("아이템창 구현 필요!");
    }

    public void OnBackButton()
    {
        PlayerData.SaveData();
        GUIManager.Instance.menuPanel.Show();
        Hide();
    }

    public void OnCharacterChangeButton(bool isRightSide)
    {
        if (charList.Count == 1) return;

        index = isRightSide ? index + 1 : index - 1;
        if (index >= charList.Count) index = 0;
        else if (index < 0) index = charList.Count - 1;
        PlayerData.currentChar = charList[index];
        BackGround.Instance.SetBackGroundCharacter();
        skillMode.Refresh(charList[index]);
    }
}
