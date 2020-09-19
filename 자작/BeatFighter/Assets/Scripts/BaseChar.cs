using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseChar : MonoBehaviour
{
    private Animator animator;

    private BaseChar target;
    public BaseChar Target { get => target; set => target = value; }

    private Stats stats;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        stats = new Stats(100);
    }

    public void Hit()
    {
        if (Target != null) Target.GetDamage(stats);
    }

    public void GetDamage(Stats from)
    {
        stats.HP -= from.Atk;
        Debug.Log(stats.HP + " " + stats.MaxHP);
    }

    public void Attack()
    {
        animator.Play("Attack", -1, 0);
    }
}
