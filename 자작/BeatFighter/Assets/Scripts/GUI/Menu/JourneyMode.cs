using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JourneyMode : PanelBase
{
    [Header("Dungeon Description")]
    [SerializeField]
    private GameObject descriptionPanel;
    [SerializeField]
    private Text mapName;
    [SerializeField]
    private Text description;
    [SerializeField]
    private Text bossName;
    [SerializeField]
    private Text note;

    private DungeonListContent dungeon;
    public int mapID { get; private set; }

    public override void Initialize() { }

    public override void Show()
    {
        base.Show();
        if (PlayerData.tutorial_journeymode == 0)
        {
            GUIManager.Instance.messageBoxPanel.CallOKMessageBox("Message_Tutorial_JourneyMode", () => { PlayerData.tutorial_journeymode = 1; PlayerData.SaveData(); });
        }
    }

    public override void Hide()
    {
        this.dungeon?.Switch();
        this.descriptionPanel.SetActive(false);
        base.Hide();
    }

    protected override void Refresh()
    {
        this.dungeon = null;
        this.mapID = 0;
    }

    public void Switch(DungeonListContent dungeon, int mapID)
    {
        if (this.dungeon?.isLock == false) this.dungeon?.Switch();
        this.dungeon = dungeon;
        this.mapID = mapID;
        if (this.dungeon.isLock) SetLockedDescription();
        else SetDescription();
    }

    private void SetDescription()
    {
        this.descriptionPanel.SetActive(true);
        this.mapName.text = TableData.instance.GetString("Map_" + this.mapID);
        this.description.text = TableData.instance.GetString("Description_" + this.mapID);
        this.bossName.text = TableData.instance.GetString("Description_Boss") + TableData.instance.GetString("Boss_" + this.mapID);
        this.note.text = TableData.instance.GetString("Description_Note") + TableData.instance.GetString("Note_" + this.mapID);
    }

    private void SetLockedDescription()
    {
        this.descriptionPanel.SetActive(true);
        this.mapName.text = TableData.instance.GetString("Map_" + this.mapID);
        this.description.text = TableData.instance.GetString("Description_Unknown");
        this.bossName.text = TableData.instance.GetString("Description_Boss") + TableData.instance.GetString("Boss_Unknown");
        this.note.text = TableData.instance.GetString("Description_Note") + TableData.instance.GetString("Note_Unknown");
    }
}
