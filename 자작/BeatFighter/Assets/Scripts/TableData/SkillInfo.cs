using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfo : TableData.IData<int>
{
    public int typeID;
    public string skillType;
    public string stateName;

    public int unlockLevel = 1;
    public float cooldown = 0;
    public float duration = 0;
    public float tickTime = 0;

    public float atkMul = 1;
    public int atkAdd = 0;
    public float defMul = 1;
    public int defAdd = 0;
    public float atkMulPerLevel = 0;
    public int atkAddPerLevel = 0;
    public float defMulPerLevel = 0;
    public int defAddPerLevel = 0;

    public float periodMul = 1;
    public float speedMul = 1;

    public int Key()
    {
        return typeID;
    }
}

public class SkillSetInfo : TableData.IData<int>
{
    public int typeID;
    public int skillID0;
    public int skillID1;
    public int skillID2;
    public int skillID3;
    public int skillID4;

    public List<int> skillIDs;

    public int Key()
    {
        return typeID;
    }

    public void Setup()
    {
        skillIDs = new List<int>();

        Add(skillID0);
        Add(skillID1);
        Add(skillID2);
        Add(skillID3);
        Add(skillID4);
    }

    private void Add(int skillID)
    {
        if (skillID > 0) skillIDs.Add(skillID);
    }
}