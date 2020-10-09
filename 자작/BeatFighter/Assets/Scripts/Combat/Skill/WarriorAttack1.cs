using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorAttack1 : Skill
{
    protected override void OnOneShot()
    {
        DoSkillDamage(castInfo.to);
        Despawn();
    }
}
