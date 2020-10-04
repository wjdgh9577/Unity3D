using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobInfo : TableData.IData<int>
{
    // Model info
    public int typeID;
    public int modelID;

    // Status info
    public int atk;
    public int def;
    public int vit;
    public int atkPerLevel;
    public int defPerLevel;
    public int vitPerLevel;

    public float signPeriod;

    public int Key()
    {
        return typeID;
    }

    public Dictionary<string, float> ReturnStats()
    {
        Dictionary<string, float> stats = new Dictionary<string, float>();
        stats["atk"] = atk;
        stats["def"] = def;
        stats["vit"] = vit;
        stats["atkPL"] = atkPerLevel;
        stats["defPL"] = defPerLevel;
        stats["vitPL"] = vitPerLevel;
        stats["signP"] = signPeriod;

        return stats;
    }
}
