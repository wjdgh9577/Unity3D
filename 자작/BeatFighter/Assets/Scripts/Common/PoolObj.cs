using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObj : MonoBehaviour
{
    public Transform ParentTM { get { return transform.parent.transform; } }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public virtual void Despawn()
    {
        PoolingManager.Instance.Despawn(gameObject);
    }
}
