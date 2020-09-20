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

    private Stats stats;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        stats = new Stats(100);
    }

    public virtual void SetTarget(BaseChar target)
    {
        Target = target;
    }

    public void Hit()
    {
        if (Target == null) return;
        DoDamage(Target);
    }

    public void DoDamage(BaseChar target)
    {
        int damage = Formula.CalcDamage(stats, target.stats);
        target.GetDamage(damage);
    }

    public void GetDamage(int damage)
    {
        stats.HP = Mathf.Clamp(stats.HP - damage, 0, stats.MaxHP);
        onTakeDamage();
        Debug.Log(stats.HP + "/" + stats.MaxHP);
    }

    public void Attack()
    {
        animator.Play("Attack", -1, 0);
    }

    public float GetHPAmount()
    {
        return (float)stats.HP / stats.MaxHP;
    }
}
