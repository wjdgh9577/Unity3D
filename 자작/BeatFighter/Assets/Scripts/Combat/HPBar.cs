using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 각 개체의 HP바 관리
/// </summary>
public class HPBar : MonoBehaviour
{
    public Image currentHP;

    /// <summary>
    /// HP바 Fill Amount 설정
    /// </summary>
    /// <param name="amount"></param>
    public void SetHPBar(float amount)
    {
        currentHP.fillAmount = amount;
    }
}
