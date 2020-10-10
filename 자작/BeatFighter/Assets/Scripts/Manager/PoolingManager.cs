using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : Singleton<PoolingManager>
{
    private Dictionary<int, List<PoolObj>> poolingObjs;
    private Dictionary<GameObject, List<PoolObj>> poolingObjs2;

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
        if (poolingObjs == null) poolingObjs = new Dictionary<int, List<PoolObj>>();

        if (poolingObjs.ContainsKey(typeID) && poolingObjs[typeID].Count > 0)
        {
            PoolObj pooledObj = poolingObjs[typeID][0];
            poolingObjs[typeID].RemoveAt(0);
            pooledObj.SetActive(true);
            pooledObj.transform.SetParent(parent);
            pooledObj.transform.localPosition = Vector3.zero;

            return pooledObj.GetComponent<T>();
        }

        if (!PreloadManager.Instance.preloadObjs.ContainsKey(typeID)) return default(T);
        GameObject spawnObj = Instantiate(PreloadManager.Instance.preloadObjs[typeID], parent);

        return spawnObj.GetComponent<T>();
    }

    /// <summary>
    /// 해당 프리팹을 스폰
    /// 풀링 리스트에 없는 경우 생성하여 반환
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public T Spawn<T>(GameObject obj, Transform parent = null)
    {
        if (poolingObjs2 == null) poolingObjs2 = new Dictionary<GameObject, List<PoolObj>>();

        if (poolingObjs2.ContainsKey(obj) && poolingObjs2[obj].Count > 0)
        {
            PoolObj pooledObj = poolingObjs2[obj][0];
            poolingObjs2[obj].RemoveAt(0);
            pooledObj.SetActive(true);
            pooledObj.transform.SetParent(parent);
            pooledObj.transform.localPosition = Vector3.zero;

            return pooledObj.GetComponent<T>();
        }

        GameObject spawnObj = Instantiate(obj, parent);
        spawnObj.GetComponent<PoolObj>().prefeb = obj;
        
        return spawnObj.GetComponent<T>();
    }

    /// <summary>
    /// 해당 오브젝트를 풀링하고 비활성화
    /// </summary>
    /// <param name="obj"></param>
    public void Despawn(PoolObj obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);

        //int typeID = int.Parse(obj.name.Split('_')[0]);
        if (int.TryParse(obj.name.Split('_')[0], out int typeID))
        {
            if (poolingObjs.ContainsKey(typeID)) poolingObjs[typeID].Add(obj);
            else poolingObjs[typeID] = new List<PoolObj>() { obj };
        }
        else
        {
            if (poolingObjs2.ContainsKey(obj.prefeb)) poolingObjs2[obj.prefeb].Add(obj);
            else poolingObjs2[obj.prefeb] = new List<PoolObj>() { obj };
        }
    }
}
