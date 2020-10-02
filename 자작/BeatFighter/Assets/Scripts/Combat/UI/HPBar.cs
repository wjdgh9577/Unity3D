using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 각 개체의 HP바 관리
/// </summary>
public class HPBar : PoolObj
{
    public Image currentHP;
    private BaseChar owner;
    private RectTransform TM;

    private void Start()
    {
        TM = GetComponent<RectTransform>();
    }

    private void Update()
    {
        TM.position = Camera.main.WorldToScreenPoint(owner.transform.parent.position + Vector3.up * owner.GetHeight());
    }

    public void Initialize(BaseChar owner)
    {
        this.owner = owner;
        Refresh();
    }

    /// <summary>
    /// HP바 Fill Amount 설정하고 체력이 0일 경우 제거
    /// </summary>
    /// <param name="amount"></param>
    public bool Refresh()
    {
        currentHP.fillAmount = owner.GetHPAmount();
        if (currentHP.fillAmount == 0)
        {
            Despawn();
            return true;
        }
        return false;
    }
}
