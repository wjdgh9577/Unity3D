using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : Singleton<BackGround>
{
    private List<Field> fields;
    private BaseChar backGroundChar;

    private float time = 0;
    private int index = 0;

    protected override void Awake()
    {
        Combat.onMapSet += Hide;
    }

    private void Update()
    {
        if (fields == null || fields.Count == 0) return;

        time += Time.deltaTime;

        if (time > 5)
        {
            time = 0;
            ChangeBackGround();
        }
    }

    public IEnumerator SetBackGround()
    {
        gameObject.SetActive(true);
        fields = new List<Field>();

        for (int i = 1000; i < 2000; i++)
        {
            Field field = PoolingManager.Instance.Spawn<Field>(i, transform);
            if (field == null) continue;
            fields.Add(field);
            field.SetActive(false);
        }
        fields[0].SetActive(true);
        yield return null;
    }

    private void ChangeBackGround()
    {
        fields[index].SetActive(false);
        if (index == fields.Count - 1) index = -1;
        index += 1;
        fields[index].SetActive(true);
    }

    public void SetBackGroundCharacter()
    {
        DeleteBackGroundCharacter();
        CharInfo info = TableData.instance.charDataDic[PlayerData.currentChar];
        backGroundChar = PoolingManager.Instance.Spawn<BaseChar>(info.modelID, transform);
    }

    public void DeleteBackGroundCharacter()
    {
        backGroundChar?.Despawn();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        time = 0;
        gameObject.SetActive(false);
    }
}
