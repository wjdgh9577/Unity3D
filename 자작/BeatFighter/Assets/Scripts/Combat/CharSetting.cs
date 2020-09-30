﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSetting : MonoBehaviour
{
    public PlayerChar SetPlayer(int typeID)
    {
        CharInfo info = TableData.instance.charDataDic[typeID];
        PlayerChar player = PoolingManager.Instance.Spawn<PlayerChar>(info.modelID, transform);
        player.stats.SetBaseStats(info.ReturnStats(), PlayerData.level);
        
        return player;
    }

    public MobChar SetMob(int typeID)
    {
        MobInfo info = TableData.instance.mobDataDic[typeID];
        MobChar mob = PoolingManager.Instance.Spawn<MobChar>(info.modelID, transform);
        mob.stats.SetBaseStats(info.ReturnStats());

        return mob;
    }
}
