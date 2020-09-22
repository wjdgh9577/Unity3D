using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageInfo
{
    BaseChar from;
    BaseChar to;
    int damage;

    public DamageInfo(BaseChar from, BaseChar to, int damage)
    {
        this.from = from;
        this.to = to;
        this.damage = damage;
    }
}

public class BaseChar : MonoBehaviour
{
    public static Action onTakeDamage;

    private Animator animator;

    private BaseChar _target;
    public BaseChar Target { get => _target; set => _target = value; }

    public Stats stats;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
    /// 애니메이션 이벤트 함수
    /// 현재 타겟에게 데미지
    /// </summary>
    public void Hit()
    {
        if (Target == null) return;
        DoDamage(Target);
    }

    /// <summary>
    /// 상대에게 데미지
    /// </summary>
    /// <param name="target"></param>
    public void DoDamage(BaseChar target)
    {
        int damage = Formula.CalcDamage(stats, target.stats);
        target.GetDamage(damage);
    }

    /// <summary>
    /// 데미지 적용
    /// </summary>
    /// <param name="damage"></param>
    public void GetDamage(int damage)
    {
        stats.hp = Mathf.Clamp(stats.hp - damage, 0, stats.maxHp);
        onTakeDamage();
        Debug.Log(stats.hp + "/" + stats.maxHp);
    }

    public void Attack()
    {
        animator.Play("Attack", -1, 0);
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
