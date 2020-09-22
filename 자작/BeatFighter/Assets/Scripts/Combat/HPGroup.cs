using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전투시 모든 HP바 관리
/// </summary>
public class HPGroup : Singleton<HPGroup>
{
    public RectTransform playerHP;
    public RectTransform[] enemiesHP;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        BaseChar.onTakeDamage += Refresh;
    }

    /// <summary>
    /// HP바 갱신
    /// </summary>
    public void Refresh()
    {
        BaseChar player = Combat.Instance.player;
        playerHP.gameObject.SetActive(player != null);
        playerHP.position = Camera.main.WorldToScreenPoint(Combat.Instance.player.transform.parent.position) + Vector3.up * 75;
        playerHP.GetComponent<HPBar>().SetHPBar(player.GetHPAmount());
        BaseChar[] enemies = Combat.Instance.mobs;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == null)
            {
                enemiesHP[i].gameObject.SetActive(false);
                continue;
            }
            enemiesHP[i].gameObject.SetActive(true);
            enemiesHP[i].position = Camera.main.WorldToScreenPoint(enemies[i].transform.parent.position) + Vector3.up * 75;
            enemiesHP[i].GetComponent<HPBar>().SetHPBar(enemies[i].GetHPAmount());
        }
    }
}
