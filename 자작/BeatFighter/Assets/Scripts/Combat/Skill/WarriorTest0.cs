using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorTest0 : Skill
{
    protected override void OnOneShot()
    {
        DoSkillDamage(castInfo.to);
        Despawn();
    }
}
