using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : PoolObj
{
    [SerializeField]
    private float rotationSpeed;

    public override void OnSpawn()
    {
        base.OnSpawn();
        CombatManager.onStageSet += Show;
        CombatManager.onStageEnd += Hide;
    }

    public override void Despawn()
    {
        CombatManager.onStageSet -= Show;
        CombatManager.onStageEnd -= Hide;
        base.Despawn();
    }

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.forward, rotationSpeed);
    }

    public void Show()
    {
        SetActive(true);
    }

    public void Hide()
    {
        SetActive(false);
    }
}
