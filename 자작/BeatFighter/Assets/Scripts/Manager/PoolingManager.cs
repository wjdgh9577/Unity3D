using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Folder
{
    UI,
    Model,
    Particle,
    Field,
    Skill
}

public class PoolingManager : Singleton<PoolingManager>
{
    private Dictionary<string, List<GameObject>> poolingObjs;

    protected override void Awake()
    {
        base.Awake();
        poolingObjs = new Dictionary<string, List<GameObject>>();
    }

    /// <summary>
    /// 해당 타입의 프리팹을 스폰
    /// 풀링 리스트에 없는 경우 생성하여 반환
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="folder"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public T Spawn<T>(string name, Folder folder, Transform parent = null)
    {
        string folderName;
        string cloneName = name + "(Clone)";

        if (poolingObjs.ContainsKey(cloneName) && poolingObjs[cloneName].Count > 0)
        {
            GameObject pooledObj = poolingObjs[cloneName][0];
            poolingObjs[cloneName].RemoveAt(0);
            pooledObj.SetActive(true);
            pooledObj.transform.SetParent(parent);

            return pooledObj.GetComponent<T>();
        }

        switch(folder)
        {
            case Folder.Model:
                folderName = "Model/";
                break;
            case Folder.UI:
                folderName = "UI/";
                break;
            case Folder.Particle:
                folderName = "Particle/";
                break;
            case Folder.Field:
                folderName = "Field/";
                break;
            case Folder.Skill:
                folderName = "Skill/";
                break;
            default:
                folderName = "";
                break;
        }

        Object obj = Resources.Load(folderName + name);
        GameObject spawnObj = Instantiate(obj, parent) as GameObject;

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
        if (poolingObjs.ContainsKey(obj.name)) poolingObjs[obj.name].Add(obj);
        else poolingObjs[obj.name] = new List<GameObject>() { obj };
        //Destroy(obj);
    }
}
