using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreloadManager : Singleton<PreloadManager>
{
    private Dictionary<int, GameObject> preloadObjs;
    private Dictionary<int, Sprite> preloadSprites;

    public IEnumerator PreloadResources()
    {
        if (preloadObjs == null) preloadObjs = new Dictionary<int, GameObject>();
        if (preloadSprites == null) preloadSprites = new Dictionary<int, Sprite>();

        GUIManager.Instance.ShowLoading();
        GUIManager.Instance.SetLoadingText("TableData Loading\n16%");
        TableData.instance = new TableData();
        TableData.instance.LoadTableDatas();

        GUIManager.Instance.SetLoadingText("UI Loading\n32%");
        yield return StartCoroutine(LoadAll("UI"));

        GUIManager.Instance.SetLoadingText("Model Loading\n48%");
        yield return StartCoroutine(LoadAll("Model"));

        GUIManager.Instance.SetLoadingText("Particle Loading\n65%");
        yield return StartCoroutine(LoadAll("Particle"));

        GUIManager.Instance.SetLoadingText("Field Loading\n81%");
        yield return StartCoroutine(LoadAll("Field"));

        GUIManager.Instance.SetLoadingText("Skill Loading\n95%");
        yield return StartCoroutine(LoadAll("Skill"));

        GUIManager.Instance.SetLoadingText("SkillIcon Loading\n95%");
        yield return StartCoroutine(LoadAllSprites("SkillIcon"));

        GUIManager.Instance.SetLoadingText("BackGround Loading\n95%");
        yield return StartCoroutine(BackGround.Instance.SetBackGround());

        GUIManager.Instance.SetLoadingText("Loading Complete\n100%");
        yield return new WaitForSeconds(0.3f);
        GUIManager.Instance.HideLoading();
    }

    private IEnumerator LoadAll(string path)
    {
        GameObject[] objs = Resources.LoadAll<GameObject>(path);
        for (int i = 0; i < objs.Length; i++)
        {
            int typeID = int.Parse(objs[i].name.Split('_')[0]);
            preloadObjs.Add(typeID, objs[i]);
        }
        yield return null;
    }

    private IEnumerator LoadAllSprites(string path)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(path);
        for (int i = 0; i < sprites.Length; i++)
        {
            int typeID = int.Parse(sprites[i].name.Split('_')[0]);
            preloadSprites.Add(typeID, sprites[i]);
        }
        yield return null;
    }

    public GameObject TryGetGameObject(int typeID)
    {
        if (preloadObjs.TryGetValue(typeID, out GameObject obj)) return obj;
        return null;
    }

    public Sprite TryGetSprite(int typeID)
    {
        if (preloadSprites.TryGetValue(typeID, out Sprite spt)) return spt;
        return null;
    }
}
