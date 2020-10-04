using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo : TableData.IData<int>
{
    public int typeID;
    public int mob1;
    public int mob1Level;
    public int mob2;
    public int mob2Level;
    public int mob3;
    public int mob3Level;
    public int mob4;
    public int mob4Level;
    public int mob5;
    public int mob5Level;

    public Dictionary<int, List<int>> mobList;

    public int Key()
    {
        return typeID;
    }

    public void Setup()
    {
        mobList = new Dictionary<int, List<int>>();

        Add(0, mob1, mob1Level);
        Add(1, mob2, mob2Level);
        Add(2, mob3, mob3Level);
        Add(3, mob4, mob4Level);
        Add(4, mob5, mob5Level);
    }

    private void Add(int place, int mobID, int mobLv)
    {
        if (mobID > 0) mobList[place] = new List<int>() { mobID, mobLv };
    }
}
