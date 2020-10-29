using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Formula
{
    public static int CalcDamage(Stats from, Stats to, JudgeRank judge, int combo)
    {
        return judge == JudgeRank.Fail ? 0 : (int)Mathf.Clamp((from.atk - to.def) * (1 + 0.1f * Mathf.Clamp((combo - 1), 0, 10)) * (judge == JudgeRank.critical ? 1.5f : 1), 0, float.PositiveInfinity);
    }

    public static float CalcAtkMul(float atk, float atkPerLevel, int level)
    {
        return atk + atkPerLevel * (level - 1);
    }

    public static int CalcAtkAdd(int atk, int atkPerLevel, int level)
    {
        return atk + atkPerLevel * (level - 1);
    }
}
