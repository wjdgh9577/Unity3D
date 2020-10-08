using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PanelBase : MonoBehaviour
{
    private void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public virtual void Show()
    {
        SetActive(true);
    }

    public virtual void Hide()
    {
        SetActive(false);
    }
}
