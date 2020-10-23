using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChar : BaseChar
{
    public static Action onSkillPrepared;
    public static Action onSkillInitialized;

    /// <summary>
    /// 공격 대상에게 타겟 표시 활성화
    /// </summary>
    /// <param name="target"></param>
    public override void SetTarget(BaseChar target)
    {
        base.SetTarget(target);
        if (target != null) CombatManager.Instance.SetTarget(target);
    }

    public override void Hit(int skillHash = 0)
    {
        SkillHash hash = (SkillHash)skillHash;
        if (hash == SkillHash.Finish || hash == SkillHash.JustOneShot) onSkillInitialized();
        base.Hit(skillHash);
    }

    public override void DoSkill(int typeID, JudgeRank judge = JudgeRank.Normal, int combo = 1)
    {
        base.DoSkill(typeID, judge, combo);
        onSkillPrepared();
    }
}
