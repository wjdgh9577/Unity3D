using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo : TableData.IData<int>
{
    public int typeID;

    public int fieldID;
    public int rewardID;

    public int stage1;
    public int stage2;
    public int stage3;
    public int stage4;
    public int stage5;

    public List<int> stageList;

    public int Key()
    {
        return typeID;
    }

    public void Setup()
    {
        stageList = new List<int>();

        Add(stage1);
        Add(stage2);
        Add(stage3);
        Add(stage4);
        Add(stage5);
    }

    private void Add(int stage)
    {
        if (stage > 0) stageList.Add(stage);
    }
}
