using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo : TableData.IData<int>
{
    public int typeID;
    public int mob1;
    public int mob2;
    public int mob3;
    public int mob4;
    public int mob5;

    public Dictionary<int, int> mobList;

    public int Key()
    {
        return typeID;
    }

    public void Setup()
    {
        mobList = new Dictionary<int, int>();

        Add(0, mob1);
        Add(1, mob2);
        Add(2, mob3);
        Add(3, mob4);
        Add(4, mob5);
    }

    private void Add(int place, int mob)
    {
        if (mob > 0) mobList[place] = mob;
    }
}
