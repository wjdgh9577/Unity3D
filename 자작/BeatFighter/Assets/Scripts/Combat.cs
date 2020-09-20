using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : Singleton<Combat>
{
    public GameObject targeting;

    public BaseChar player;
    public BaseChar[] enemies = new BaseChar[5];

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindObjectOfType<PlayerChar>();
        enemies = GameObject.FindObjectsOfType<MobChar>();
    }

    public void SetTarget(BaseChar target)
    {
        targeting.SetActive(true);
        targeting.transform.position = target.transform.position + Vector3.up * 0.1f;
    }
}
