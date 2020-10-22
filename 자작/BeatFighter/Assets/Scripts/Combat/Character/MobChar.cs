using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobChar : BaseChar
{
    private Dictionary<SkillInfo, float> skills;
    private float lastSignTime;

    private void Update()
    {
        if (CombatManager.Instance.isCombat && !isDead && CombatManager.Targetable(Target))
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

        if (skills == null) skills = new Dictionary<SkillInfo, float>();
        skills.Clear();
        foreach (int id in skillIDs)
        {
            skills.Add(TableData.instance.skillDataDic[id], lastSignTime);
        }
    }
}
