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

    public override void Initialize()
    {
        this.skillMode.Initialize();
    }

    /// <summary>
    /// 판넬 활성화시 초기화
    /// </summary>
    protected override void Refresh()
    {
        this.charDic = PlayerData.charDataDic;
        if (this.charList == null) this.charList = new List<int>();
        this.charList.Clear();
        foreach (var key in this.charDic.Keys)
        {
            this.charList.Add(key);
        }
        this.charList.Sort();
        this.index = this.charList.IndexOf(PlayerData.currentChar);
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

        this.statText.text = string.Format(TableData.instance.GetString("Collection_Stats"), name, level, exp, vit, atk, def, signPeriod, signSpeed);
    }

    public void OnSkillButton()
    {
        this.skillMode.Show();
        this.skillMode.Refresh(this.charList[index]);
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
        if (this.charList.Count == 1) return;

        this.index = isRightSide ? this.index + 1 : this.index - 1;
        if (this.index >= this.charList.Count) this.index = 0;
        else if (this.index < 0) this.index = this.charList.Count - 1;
        PlayerData.ChangeCurrentChar(this.charList[this.index]);
        BackGround.Instance.SetBackGroundCharacter();
        SetStats();
    }
}
