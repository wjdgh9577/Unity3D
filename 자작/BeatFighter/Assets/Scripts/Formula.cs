using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Formula
{
    public static int CalcDamage(Stats from, Stats to)
    {
        return (int)Mathf.Clamp(from.Atk - to.def, 0, float.PositiveInfinity);
    }
}
