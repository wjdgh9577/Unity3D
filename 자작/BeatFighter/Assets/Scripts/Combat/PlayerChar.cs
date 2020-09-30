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
        Combat.Instance.SetTarget(target);
    }

    public override void Hit()
    {
        base.Hit();
        onSkillInitialized();
    }

    public override void DoSkill(int typeID, JudgeRank judge = JudgeRank.Normal)
    {
        base.DoSkill(typeID, judge);
        onSkillPrepared();
    }
}
