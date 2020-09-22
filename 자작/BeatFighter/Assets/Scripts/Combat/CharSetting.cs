using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSetting : MonoBehaviour
{
    public PlayerChar SetPlayer(int typeID)
    {
        CharInfo info = TableData.instance.charDataDic[typeID];
        PlayerChar player = PoolingManager.Instance.SpawnPlayer(info.model, transform);
        player.stats.SetBaseStats(info.vit, info.atk, info.def);
        
        return player;
    }

    public MobChar SetMob(int typeID)
    {
        MobInfo info = TableData.instance.mobDataDic[typeID];
        MobChar mob = PoolingManager.Instance.SpawnMob(info.model, transform);
        mob.stats.SetBaseStats(info.vit, info.atk, info.def);

        return mob;
    }
}
