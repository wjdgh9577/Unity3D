using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorTest : Skill
{
    protected override void OnStart()
    {
        DoSkillDamage(castInfo.to);
    }

    protected override void OnMiddle()
    {
        DoSkillDamage(castInfo.to);
    }

    protected override void OnFinish()
    {
        DoSkillDamageAllEnemies();
        Despawn();
    }

    protected override void OnAlive()
    {
        if (!Combat.Targetable(castInfo.from)) Despawn();
    }
}
