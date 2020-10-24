using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : Singleton<PoolingManager>
{
    private Dictionary<GameObject, Queue<PoolObj>> poolingObjs;

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
        if (poolingObjs == null) poolingObjs = new Dictionary<GameObject, Queue<PoolObj>>();

        GameObject obj = PreloadManager.Instance.TryGetGameObject(typeID);
        T spawnObj = Spawn<T>(obj, parent);

        return spawnObj;
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
        if (obj == null) return default(T);
        
        if (poolingObjs.ContainsKey(obj) && poolingObjs[obj].Count > 0)
        {
            PoolObj pooledObj = poolingObjs[obj].Dequeue();
            pooledObj.SetActive(true);
            pooledObj.OnSpawn();
            pooledObj.SetAudioVolume();
            pooledObj.transform.SetParent(parent);
            pooledObj.transform.localPosition = Vector3.zero;

            return pooledObj.GetComponent<T>();
        }

        PoolObj spawnObj = Instantiate(obj, parent).GetComponent<PoolObj>();
        spawnObj.prefeb = obj;
        spawnObj.OnSpawn();
        spawnObj.SetAudioVolume();
        spawnObj.transform.localPosition = Vector3.zero;

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

        if (poolingObjs.ContainsKey(obj.prefeb)) poolingObjs[obj.prefeb].Enqueue(obj);
        else
        {
            poolingObjs[obj.prefeb] = new Queue<PoolObj>();
            poolingObjs[obj.prefeb].Enqueue(obj);
        }
    }
}
