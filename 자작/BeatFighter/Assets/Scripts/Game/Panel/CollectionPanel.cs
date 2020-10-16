using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionPanel : PanelBase
{
    public SkillMode skillMode;
    public Text statText;

    private Dictionary<int, PlayerData.CharData> charDic;
    private List<int> charList;
    private int index;

    /// <summary>
    /// 판넬 활성화시 초기화
    /// </summary>
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
        SetStats();
    }

    private void SetStats()
    {
        CharInfo charInfo = TableData.instance.charDataDic[PlayerData.currentChar];
        PlayerData.CharData charData = PlayerData.charDataDic[PlayerData.currentChar];
        CharExpInfo expInfo = TableData.instance.charExpDataDic[charData.level];

        string name = TableData.instance.GetString(PlayerData.currentChar.ToString());
        int level = charData.level;
        string exp = string.Format("{0} / {1}", charData.exp, expInfo.requireExp);
        int vit = charInfo.vit + (level - 1) * charInfo.vitPerLevel;
        int atk = charInfo.atk + (level - 1) * charInfo.atkPerLevel;
        int def = charInfo.def + (level - 1) * charInfo.defPerLevel;
        float signPeriod = charInfo.signPeriod;
        float signSpeed = charInfo.signSpeed;

        statText.text = string.Format(TableData.instance.GetString("Collection_Stats"), name, level, exp, vit, atk, def, signPeriod, signSpeed);
    }

    public void OnSkillButton()
    {
        skillMode.Show();
        skillMode.Refresh(charList[index]);
    }

    public void OnItemButton()
    {
        GUIManager.Instance.messageBoxPanel.CallOKMessageBox("Develop", null);
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
        PlayerData.ChangeCurrentChar(charList[index]);
        BackGround.Instance.SetBackGroundCharacter();
        SetStats();
    }
}
