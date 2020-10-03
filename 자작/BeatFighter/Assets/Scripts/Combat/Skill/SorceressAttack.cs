using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SorceressAttack : Skill
{
    protected override void OnOneShot()
    {
        Play("Explosion", castInfo.to.transform.position);
        DoSkillDamage(castInfo.to);
        Despawn();
    }
}
