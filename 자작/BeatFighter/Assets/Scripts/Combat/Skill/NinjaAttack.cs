using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaAttack : Skill
{
    protected override void OnOneShot()
    {
        DoSkillDamage(castInfo.to);
        Despawn();
    }
}
