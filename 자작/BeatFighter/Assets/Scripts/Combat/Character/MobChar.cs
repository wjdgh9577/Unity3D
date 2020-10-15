using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobChar : BaseChar
{
    private Dictionary<SkillInfo, float> skills;
    private float lastSignTime;
    private bool isCombat = false;

    private void Update()
    {
        if (isCombat && !isDead && Combat.Targetable(Target))
        {
            if (Time.time - lastSignTime >= stats.signPeriod)
            {
                foreach (var pair in skills)
                {
                    if (Time.time - pair.Value >= pair.Key.cooldown)
                    {
                        DoSkill(pair.Key.Key());
                        skills[pair.Key] = Time.time;
                        break;
                    }
                }
                lastSignTime = Time.time;
            }
        }
    }

    public void StartCombat()
    {
        lastSignTime = Time.time;
        isCombat = true;

        if (skills == null) skills = new Dictionary<SkillInfo, float>();
        skills.Clear();
        foreach (int id in skillIDs)
        {
            skills.Add(TableData.instance.skillDataDic[id], lastSignTime);
        }
    }

    public override void Despawn()
    {
        isCombat = false;
        base.Despawn();
    }
}
