using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : PoolObj
{
    public GameObject bossTrigger;
    private BossTrigger obj;

    public override void Despawn()
    {
        obj.Despawn();
        base.Despawn();
    }

    public void SetBossTrigger(int stageCount)
    {
        obj = PoolingManager.Instance.Spawn<BossTrigger>(bossTrigger, this.transform);
        obj.LocalPosition(stageCount * 20 - 10, 0, 0);
    }
}
