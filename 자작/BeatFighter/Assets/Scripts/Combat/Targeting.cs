using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : PoolObj
{
    [SerializeField]
    private float rotationSpeed;

    private void Awake()
    {
        Combat.onStageSet += Show;
        Combat.onStageEnd += Hide;
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
