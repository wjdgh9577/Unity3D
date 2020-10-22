using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PanelBase : MonoBehaviour
{
    public abstract void Initialize();

    private void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    protected virtual void Refresh() { }

    public virtual void Show()
    {
        SetActive(true);
        Refresh();
    }

    public virtual void Hide()
    {
        SetActive(false);
    }
}
