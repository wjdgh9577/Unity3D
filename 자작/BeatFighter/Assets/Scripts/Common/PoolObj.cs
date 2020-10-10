using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObj : MonoBehaviour
{
    public Transform ParentTM { get { return transform.parent.transform; } }
    [System.NonSerialized]
    public GameObject prefeb;

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public virtual void Despawn()
    {
        PoolingManager.Instance.Despawn(this);
    }
}
