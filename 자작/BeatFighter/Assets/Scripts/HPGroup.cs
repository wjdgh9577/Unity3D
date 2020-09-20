using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        BaseChar.onTakeDamage += SetHP;
    }

    public void SetHP()
    {
        BaseChar player = Combat.Instance.player;
        playerHP.gameObject.SetActive(player != null);
        playerHP.position = Camera.main.WorldToScreenPoint(Combat.Instance.player.transform.parent.position) + Vector3.up * 75;
        playerHP.GetComponent<HPBar>().SetHPBar(player.GetHPAmount());
        BaseChar[] enemies = Combat.Instance.enemies;
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
