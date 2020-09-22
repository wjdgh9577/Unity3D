using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : Singleton<PoolingManager>
{
    public PlayerChar SpawnPlayer(string model, Transform parent)
    {
        Object obj = Resources.Load("PlayerChar/" + model);
        GameObject player = Instantiate(obj, parent) as GameObject;
        
        return player.GetComponent<PlayerChar>();
    }

    public MobChar SpawnMob(string model, Transform parent)
    {
        Object obj = Resources.Load("MobChar/" + model);
        GameObject mob = Instantiate(obj, parent) as GameObject;
        
        return mob.GetComponent<MobChar>();
    }
}
