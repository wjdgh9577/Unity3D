using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorTest3 : Skill
{
    protected override void OnOneShot()
    {
        DoSkillDamage(castInfo.to);
        Despawn();
    }
}
