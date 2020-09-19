using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Stats
{
    public int MaxHP;
    public int HP;

    public int Atk;
    public int def;

    public Stats(int MaxHP)
    {
        this.MaxHP = MaxHP;
        this.HP = MaxHP;
        this.Atk = 1;
        this.def = 0;
    }
}
