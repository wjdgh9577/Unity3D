using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Folder
{
    UI,
    Model,
    Particle,
    Field
}

public class PoolingManager : Singleton<PoolingManager>
{
    public T Spawn<T>(string name, Folder folder, Transform parent = null)
    {
        string folderName;

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
            default:
                folderName = "";
                break;
        }

        Object obj = Resources.Load(folderName + name);
        GameObject noteGObj = Instantiate(obj, parent) as GameObject;

        return noteGObj.GetComponent<T>();
    }

    public void Despawn(GameObject obj)
    {
        Destroy(obj);
    }
}
