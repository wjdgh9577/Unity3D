﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public void Despawn()
    {
        PoolingManager.Instance.Despawn(gameObject);
    }
}
