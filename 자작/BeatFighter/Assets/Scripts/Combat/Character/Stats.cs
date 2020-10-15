using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Stats
{
    public int level;

    public int maxHp;
    public int hp;

    public int atk;
    public int def;
    public int vit;

    public int atkPerLevel;
    public int defPerLevel;
    public int vitPerLevel;

    public float signPeriod;
    public float signSpeed;

    public void SetBaseStats(Dictionary<string, float> stats, int level = 0)
    {
        this.level = level;
        this.atkPerLevel = (int)stats["atkPL"];
        this.defPerLevel = (int)stats["defPL"];
        this.vitPerLevel = (int)stats["vitPL"];
        this.atk = (int)stats["atk"] + (this.level - 1) * this.atkPerLevel;
        this.def = (int)stats["def"] + (this.level - 1) * this.defPerLevel;
        this.maxHp = (int)stats["vit"] + (this.level - 1) * this.vitPerLevel;
        this.hp = this.maxHp;
        this.signPeriod = stats["signP"];
        this.signSpeed = stats.ContainsKey("signS") ? stats["signS"] : 0;
    }

    public void AddSkillParameter(SkillInfo info, int skillLevel)
    {
        float atkMul = info.atkMul + (skillLevel - 1) * info.atkMulPerLevel;
        int atkAdd = info.atkAdd + (skillLevel - 1) * info.atkAddPerLevel;
        float defMul = info.defMul + (skillLevel - 1) * info.defMulPerLevel;
        int defAdd = info.defAdd + (skillLevel - 1) * info.defAddPerLevel;
        atk = (int)(atk * atkMul) + atkAdd;
        def = (int)(def * defMul) + defAdd;
        signPeriod *= info.periodMul;
        signSpeed *= info.speedMul;
    }

    public List<float> GetSign()
    {
        List<float> sign = new List<float>() { signPeriod * Mathf.Clamp((float)hp / maxHp, 0.5f, 1f), signSpeed * Mathf.Clamp((float)maxHp / hp, 1f, 2f) };
        return sign;
    }
}
