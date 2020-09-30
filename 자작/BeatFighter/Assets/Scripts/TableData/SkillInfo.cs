using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfo : TableData.IData<int>
{
    public int typeID;
    public string stateName;

    public float cooldown = 0;
    public float atkMul = 1;
    public float atkAdd = 0;
    public float defMul = 1;
    public float defAdd = 0;
    public float periodMul = 1;
    public float speedMul = 1;

    public int Key()
    {
        return typeID;
    }
}
