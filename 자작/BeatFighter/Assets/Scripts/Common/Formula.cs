using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Formula
{
    public static int CalcDamage(Stats from, Stats to, JudgeRank judge)
    {
        return judge == JudgeRank.Fail ? 0 : (int)Mathf.Clamp((from.atk - to.def) * (judge == JudgeRank.critical ? 1.5f : 1), 0, float.PositiveInfinity);
    }
}
