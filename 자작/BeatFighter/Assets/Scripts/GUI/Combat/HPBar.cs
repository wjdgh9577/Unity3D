using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 각 개체의 HP바 관리
/// </summary>
public class HPBar : PoolObj
{
    public DamageText damageText;
    public Image currentHP;
    private BaseChar owner;
    private RectTransform TM;

    private void Update()
    {
        if (!this.owner.isDead) TM.position = Camera.main.WorldToScreenPoint(this.owner.transform.parent.position + Vector3.up * this.owner.GetHeight());
    }

    public void Initialize(BaseChar owner)
    {
        TM = GetComponent<RectTransform>();

        this.owner = owner;
        this.owner.changeDamageUI += damageText.Refresh;
        Refresh();
    }

    public void CanDespawn()
    {
        this.owner.changeDamageUI -= damageText.Refresh;
        this.damageText.Despawn();
        Despawn();
    }

    /// <summary>
    /// HP바 Fill Amount 설정하고 체력이 0일 경우 제거
    /// </summary>
    /// <param name="amount"></param>
    public bool Refresh()
    {
        this.currentHP.fillAmount = this.owner.GetHPAmount();
        if (this.currentHP.fillAmount == 0)
        {
            Invoke("CanDespawn", 1);
            return true;
        }
        return false;
    }
}
