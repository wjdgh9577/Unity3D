using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전투시 모든 HP바 관리
/// </summary>
public class HPGroup : Singleton<HPGroup>
{
    private HPBar playerHPBar;
    private List<HPBar> mobHPBars;

    public void Initialize()
    {
        this.mobHPBars = new List<HPBar>();

        BaseChar.onTakeDamage += Refresh;
        CombatManager.onMapEnd += Despawn;
    }

    /// <summary>
    /// HP Group을 초기화
    /// </summary>
    public void SetHPGroup(PlayerChar player, MobChar[] mobs)
    {
        if (player == null) return;
        if (this.playerHPBar == null)
        {
            HPBar hpBar = PoolingManager.Instance.Spawn<HPBar>(PlayerData.MaxHPUI, transform);
            hpBar.Initialize(player);
            playerHPBar = hpBar;
        }
        for (int i = 0; i < mobs.Length; i++)
        {
            if (!CombatManager.Targetable(mobs[i])) continue;
            HPBar hpBar = PoolingManager.Instance.Spawn<HPBar>(PlayerData.MaxHPUI, transform);
            hpBar.Initialize(mobs[i]);
            mobHPBars.Add(hpBar);
        }
    }

    /// <summary>
    /// HP바 갱신
    /// </summary>
    public void Refresh()
    {
        bool isDespawned;

        isDespawned = this.playerHPBar.Refresh();
        if (isDespawned) this.playerHPBar = null;
        for (int i = this.mobHPBars.Count - 1; i >= 0; i--)
        {
            isDespawned = this.mobHPBars[i].Refresh();
            if (isDespawned) this.mobHPBars.RemoveAt(i);
        }
    }

    /// <summary>
    /// 모든 HP바 제거
    /// </summary>
    public void Despawn()
    {
        if (this.playerHPBar != null) this.playerHPBar.Despawn();
        this.playerHPBar = null;
        for (int i = 0; i < this.mobHPBars.Count; i++)
        {
            this.mobHPBars[i].Despawn();
        }
        this.mobHPBars.Clear();
    }
}
