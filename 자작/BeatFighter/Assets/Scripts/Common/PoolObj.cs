using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObj : MonoBehaviour
{
    public virtual void Despawn()
    {
        PoolingManager.Instance.Despawn(gameObject);
    }
}
