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

    public override void Hide()
    {
        this.dungeon?.Switch();
        descriptionPanel.SetActive(false);
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
        descriptionPanel.SetActive(true);
        mapName.text = TableData.instance.GetString("Map_" + mapID);
        description.text = TableData.instance.GetString("Description_" + mapID);
        bossName.text = TableData.instance.GetString("Description_Boss") + TableData.instance.GetString("Boss_" + mapID);
        note.text = TableData.instance.GetString("Description_Note") + TableData.instance.GetString("Note_" + mapID);
    }

    private void SetLockedDescription()
    {
        descriptionPanel.SetActive(true);
        mapName.text = TableData.instance.GetString("Map_" + mapID);
        description.text = TableData.instance.GetString("Description_Unknown");
        bossName.text = TableData.instance.GetString("Description_Boss") + TableData.instance.GetString("Boss_Unknown");
        note.text = TableData.instance.GetString("Description_Note") + TableData.instance.GetString("Note_Unknown");
    }
}
