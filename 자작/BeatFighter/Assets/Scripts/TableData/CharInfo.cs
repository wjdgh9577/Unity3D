using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharInfo : TableData.IData<int>
{
    public int typeID;
    public string model;
    public int atk;
    public int def;
    public int vit;
    public int atkPerLevel;
    public int defPerLevel;
    public int vitPerLevel;

    public int Key()
    {
        return typeID;
    }
}
