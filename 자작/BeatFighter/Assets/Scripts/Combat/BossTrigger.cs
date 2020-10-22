using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : PoolObj
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            CombatManager.Instance.EnterBossTrigger();
        }
    }
}
