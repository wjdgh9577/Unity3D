using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageInfo
{
    public BaseChar from;
    public BaseChar to;
    public int damage;
    public float criticalBonus;

    public DamageInfo(BaseChar from, BaseChar to, int damage, float criticalBonus)
    {
        this.from = from;
        this.to = to;
        this.damage = damage;
        this.criticalBonus = criticalBonus;
    }
}

public struct CastInfo
{
    public BaseChar from;
    public BaseChar to;
}

public class BaseChar : PoolObj
{
    public static Action onTakeDamage;

    private Animator animator;

    private BaseChar _target;
    public BaseChar Target { get => _target; set => _target = value; }

    public Stats stats;

    public bool isDead = false;
    private Skill currentSkill;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void CreateDmgParticle(DamageInfo info)
    {
        HPParticle particle = PoolingManager.Instance.Spawn<HPParticle>(PlayerData.HPParticle);
        particle.transform.position = transform.position;
        particle.SetMesh(info);
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
    public virtual void Hit()
    {
        if (Target == null) return;
        currentSkill.Initialize();
    }

    /// <summary>
    /// 상대에게 데미지
    /// </summary>
    /// <param name="target"></param>
    public void DoDamage(BaseChar target)
    {
        int damage = Formula.CalcDamage(stats, target.stats);
        DamageInfo info = new DamageInfo(this, target, damage, 0);
        target.GetDamage(info);
    }

    /// <summary>
    /// 데미지 적용
    /// </summary>
    /// <param name="damage"></param>
    public void GetDamage(DamageInfo info)
    {
        stats.hp = Mathf.Clamp(stats.hp - info.damage, 0, stats.maxHp);
        CreateDmgParticle(info);
        onTakeDamage();
        Debug.Log(stats.hp + "/" + stats.maxHp);
    }

    /// <summary>
    /// 스킬 사용
    /// </summary>
    /// <param name="typeID"></param>
    public virtual void DoSkill(int typeID, JudgeRank judge = JudgeRank.Normal)
    {
        CastInfo castInfo = new CastInfo() { from = this, to = Target };
        SkillInfo skillInfo = TableData.instance.skillDataDic[typeID];
        currentSkill = PoolingManager.Instance.Spawn<Skill>(typeID);
        currentSkill.Prepare(castInfo, skillInfo, judge);
    }

    /// <summary>
    /// 스킬 시전시 특정 캐스팅 동작 실행
    /// </summary>
    /// <param name="stateName"></param>
    public void Play(string stateName)
    {
        animator.Play(stateName, -1, 0);
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
