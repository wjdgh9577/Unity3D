using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorTest1 : Skill
{
    protected override void OnOneShot()
    {
        DoSkillDamage(castInfo.to);
        Despawn();
    }
}
