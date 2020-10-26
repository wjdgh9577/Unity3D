using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageInfo
{
    public BaseChar from;
    public BaseChar to;
    public int damage;
    public JudgeRank judge;

    public DamageInfo(BaseChar from, BaseChar to, int damage, JudgeRank judge)
    {
        this.from = from;
        this.to = to;
        this.damage = damage;
        this.judge = judge;
    }
}

public struct CastInfo
{
    public BaseChar from;
    public BaseChar to;
}

public class BaseChar : PoolObj
{
    protected List<int> skillIDs;

    public static Action onTakeDamage;
    public static Action onPlayerDeath;
    public static Action onMobDeath;
    public Action<DamageInfo> changeDamageUI;

    private Animator animator;

    private BaseChar _target;
    public BaseChar Target { get => _target; set => _target = value; }

    public Stats stats;

    [System.NonSerialized]
    public bool isDead = false;
    private Skill currentSkill;

    public virtual void Initialize(int typeID)
    {
        animator = GetComponent<Animator>();

        isDead = false;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        skillIDs = TableData.instance.skillSetDataDic[typeID].skillIDs;
    }

    /// <summary>
    /// 공격 대상 설정
    /// </summary>
    /// <param name="target"></param>
    public virtual void SetTarget(BaseChar target)
    {
        Target = target;
    }

    /// <summary>
    /// 콜라이더의 높이를 반환
    /// </summary>
    /// <returns></returns>
    public float GetHeight()
    {
        return GetComponent<Collider>().bounds.center.y * 2;
    }

    /// <summary>
    /// 애니메이션 이벤트 함수
    /// 현재 타겟에게 데미지
    /// </summary>
    public virtual void Hit(int skillHash = 0)
    {
        if (Target == null) return;
        SkillHash hash = (SkillHash)skillHash;
        if (hash == SkillHash.Start || hash == SkillHash.JustOneShot) currentSkill.Initialize();
        currentSkill.OnInitialized(hash);
    }

    /// <summary>
    /// 상대에게 데미지
    /// </summary>
    /// <param name="target"></param>
    public void DoDamage(BaseChar target, Stats stats, JudgeRank judge, int combo)
    {
        int damage = Formula.CalcDamage(stats, target.stats, judge, combo);
        DamageInfo info = new DamageInfo(this, target, damage, judge);
        target.GetDamage(info);
    }

    /// <summary>
    /// 데미지 적용
    /// </summary>
    /// <param name="damage"></param>
    public void GetDamage(DamageInfo info)
    {
        if (!CombatManager.Instance.isCombat) return;
        int damage = info.damage;
        stats.hp = Mathf.Clamp(stats.hp - damage, 0, stats.maxHp);
        //CombatManager.Instance.CreateDmgParticle(info);
        changeDamageUI(info);
        onTakeDamage();
        if (stats.hp <= 0)
        {
            isDead = true;
            if (this is MobChar)
            {
                onMobDeath();
            }
            else
            {
                onPlayerDeath();
            }
            CrossFade("Dead");
        }
    }

    /// <summary>
    /// 스킬 사용
    /// </summary>
    /// <param name="typeID"></param>
    public virtual void DoSkill(int typeID, JudgeRank judge = JudgeRank.Normal, int combo = 1)
    {
        CastInfo castInfo = new CastInfo() { from = this, to = Target };
        SkillInfo skillInfo = TableData.instance.skillDataDic[typeID];
        currentSkill = PoolingManager.Instance.Spawn<Skill>(typeID);
        currentSkill.Prepare(castInfo, skillInfo, judge, combo);
    }

    public virtual void Dead()
    {
        if (this is MobChar) Despawn();
    }

    /// <summary>
    /// 특정 애니메이션 스테이트를 즉시 실행
    /// </summary>
    /// <param name="stateName"></param>
    public void Play(string stateName)
    {
        animator.Play(stateName, -1, 0);
    }

    /// <summary>
    /// 특정 애니메이션 스테이트를 부드럽게 실행
    /// </summary>
    /// <param name="stateName"></param>
    public void CrossFade(string stateName)
    {
        animator.CrossFade(stateName, 0.3f);
    }

    /// <summary>
    /// 현재 HP 비율 반환
    /// </summary>
    /// <returns></returns>
    public float GetHPAmount()
    {
        return (float)stats.hp / stats.maxHp;
    }
}
