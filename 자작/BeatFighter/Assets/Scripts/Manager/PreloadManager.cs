using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreloadManager : Singleton<PreloadManager>
{
    public Dictionary<int, GameObject> preloadObjs;

    public void PreloadResources()
    {
        if (preloadObjs == null) preloadObjs = new Dictionary<int, GameObject>();

        LoadAll("UI");
        LoadAll("Model");
        LoadAll("Particle");
        LoadAll("Field");
        LoadAll("Skill");
    }

    private void LoadAll(string path)
    {
        GameObject[] objs = Resources.LoadAll<GameObject>(path);
        for (int i = 0; i < objs.Length; i++)
        {
            int typeID = int.Parse(objs[i].name.Split('_')[0]);
            preloadObjs.Add(typeID, objs[i]);
        }
    }
}
