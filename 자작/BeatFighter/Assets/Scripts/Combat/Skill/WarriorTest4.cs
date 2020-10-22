using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorTest4 : Skill
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
        if (!CombatManager.Targetable(castInfo.from)) Despawn();
    }
}
