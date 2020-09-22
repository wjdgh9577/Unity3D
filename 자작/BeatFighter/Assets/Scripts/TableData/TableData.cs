using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonFx.Json;

public class TableData
{
    public static TableData instance = null;

    public class SerializeObj<Type>
    {
        public string md5;
        public Type[] array;
    }

    public interface IData<KeyType>
    {
        KeyType Key();
    }

    private Dictionary<int, CharInfo> _charDataDic;
    private Dictionary<int, MobInfo> _mobDataDic;
    private Dictionary<int, MapInfo> _mapDataDic;
    private Dictionary<int, StageInfo> _stageDataDic;
    
    public Dictionary<int, CharInfo> charDataDic => _charDataDic;
    public Dictionary<int, MobInfo> mobDataDic => _mobDataDic;
    public Dictionary<int, MapInfo> mapDataDic => _mapDataDic;
    public Dictionary<int, StageInfo> stageDataDic => _stageDataDic;

    public void LoadTableDatas()
    {
        LoadTable<int, CharInfo>("CharTable", out _charDataDic);
        LoadTable<int, MobInfo>("MobTable", out _mobDataDic);
        LoadTable<int, MapInfo>("MapTable", out _mapDataDic);
        foreach (var pair in mapDataDic) pair.Value.Setup();
        LoadTable<int, StageInfo>("StageTable", out _stageDataDic);
        foreach (var pair in stageDataDic) pair.Value.Setup();
    }

    private void LoadTable<Key, Value>(string jsonFilename, out Dictionary<Key, Value> dataDic) where Value : IData<Key>
    {
        TextAsset jsonText = Resources.Load<TextAsset>("DataTable/" + jsonFilename);
        if (jsonText == null)
        {
            dataDic = null;
            return;
        }

        dataDic = new Dictionary<Key, Value>();

        SerializeObj<Value> data = JsonFx.Json.JsonReader.Deserialize<SerializeObj<Value>>(jsonText.text);

        for (int i = 0; i < data.array.Length; i++)
        {
            Value item = data.array[i];
            dataDic.Add(item.Key(), item);
        }
    }
}
