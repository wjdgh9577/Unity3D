using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Stats
{
    public int maxHp;
    public int hp;

    public int atk;
    public int def;

    public void SetBaseStats(int maxHp, int atk, int def)
    {
        this.maxHp = maxHp;
        this.hp = maxHp;
        this.atk = atk;
        this.def = def;
    }
}
