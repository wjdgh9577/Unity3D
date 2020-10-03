using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SorceressTest : Skill
{
    protected override void OnOneShot()
    {
        Play("Explosion", castInfo.from.transform.position + Vector3.right * 5);
        StartCoroutine(TestCoroutine());
    }

    private IEnumerator TestCoroutine()
    {
        yield return new WaitForSeconds(3.8f);
        StartTimer();
    }

    protected override void OnTick()
    {
        DoSkillDamageAllEnemies();
    }

    protected override void OnExpired()
    {
        Despawn();
    }
}
