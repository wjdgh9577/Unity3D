using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobInfo : TableData.IData<int>
{
    public int typeID;
    public string model;
    public int atk;
    public int def;
    public int vit;

    public int Key()
    {
        return typeID;
    }
}
