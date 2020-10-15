using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JourneyMode : PanelBase
{
    private DungeonListContent dungeon;
    public int mapID { get; private set; }

    public override void Hide()
    {
        this.dungeon?.Switch();
        base.Hide();
    }

    protected override void Refresh()
    {
        this.dungeon = null;
        this.mapID = 0;
    }

    public void Switch(DungeonListContent dungeon, int mapID)
    {
        this.dungeon?.Switch();
        this.dungeon = dungeon;
        this.mapID = mapID;
        SetDescription();
    }

    private void SetDescription()
    {

    }
}
