using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전투시 모든 HP바 관리
/// </summary>
public class HPGroup : Singleton<HPGroup>
{
    private List<HPBar> bars;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        bars = new List<HPBar>();
        BaseChar.onTakeDamage += Refresh;
        Combat.onStageSet += SetHPGroup;
    }

    /// <summary>
    /// HP Group을 초기화
    /// </summary>
    public void SetHPGroup()
    {
        if (Combat.Instance.player == null) return;
        HPBar hpBar = PoolingManager.Instance.Spawn<HPBar>("MaxHP", Folder.UI, this.transform);
        hpBar.Initialize(Combat.Instance.player);
        bars.Add(hpBar);
        for (int i = 0; i < Combat.Instance.mobCount; i++)
        {
            if (Combat.Instance.mobs[i] == null) continue;
            hpBar = PoolingManager.Instance.Spawn<HPBar>("MaxHP", Folder.UI, this.transform);
            hpBar.Initialize(Combat.Instance.mobs[i]);
            bars.Add(hpBar);
        }
    }

    /// <summary>
    /// HP바 갱신
    /// </summary>
    public void Refresh()
    {
        for (int i = 0; i < bars.Count; i++)
        {
            bars[i].Refresh();
        }
    }

    /// <summary>
    /// 모든 HP바 제거
    /// </summary>
    public void Despawn()
    {
        for (int i = 0; i < bars.Count; i++)
        {
            bars[i].Despawn();
        }
        bars.Clear();
    }
}
