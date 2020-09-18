using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseChar : MonoBehaviour
{
    private Animator animator;

    private BaseChar _target;
    public BaseChar Target { get => _target; set => _target = value; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        animator.Play("Attack");
    }
}
