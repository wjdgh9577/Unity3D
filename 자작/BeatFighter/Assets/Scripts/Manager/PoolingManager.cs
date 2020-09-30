using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : Singleton<PoolingManager>
{
    private Dictionary<int, List<GameObject>> poolingObjs;

    /// <summary>
    /// 해당 타입의 프리팹을 스폰
    /// 풀링 리스트에 없는 경우 생성하여 반환
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="typeID"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public T Spawn<T>(int typeID, Transform parent = null)
    {

        if (poolingObjs == null) poolingObjs = new Dictionary<int, List<GameObject>>();

        if (poolingObjs.ContainsKey(typeID) && poolingObjs[typeID].Count > 0)
        {
            GameObject pooledObj = poolingObjs[typeID][0];
            poolingObjs[typeID].RemoveAt(0);
            pooledObj.SetActive(true);
            pooledObj.transform.SetParent(parent);

            return pooledObj.GetComponent<T>();
        }

        GameObject spawnObj = Instantiate(PreloadManager.Instance.preloadObjs[typeID], parent);

        return spawnObj.GetComponent<T>();
    }

    /// <summary>
    /// 해당 오브젝트를 풀링하고 비활성화
    /// </summary>
    /// <param name="obj"></param>
    public void Despawn(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(this.transform);

        int typeID = int.Parse(obj.name.Split('_')[0]);

        if (poolingObjs.ContainsKey(typeID)) poolingObjs[typeID].Add(obj);
        else poolingObjs[typeID] = new List<GameObject>() { obj };
    }
}
