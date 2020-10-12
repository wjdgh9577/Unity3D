using System.Collections.Generic;
using UnityEngine;

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
    private Dictionary<int, SkillInfo> _skillDataDic;
    private Dictionary<int, SkillSetInfo> _skillSetDataDic;
    private Dictionary<int, RewardInfo> _rewardDataDic;
    private Dictionary<int, CharExpInfo> _charExpDataDic;
    private Dictionary<int, SkillExpInfo> _skillExpDataDic;
    private Dictionary<string, StringInfo> _stringDataDic;
    
    public Dictionary<int, CharInfo> charDataDic => _charDataDic;
    public Dictionary<int, MobInfo> mobDataDic => _mobDataDic;
    public Dictionary<int, MapInfo> mapDataDic => _mapDataDic;
    public Dictionary<int, StageInfo> stageDataDic => _stageDataDic;
    public Dictionary<int, SkillInfo> skillDataDic => _skillDataDic;
    public Dictionary<int, SkillSetInfo> skillSetDataDic => _skillSetDataDic;
    public Dictionary<int, RewardInfo> rewardDataDic => _rewardDataDic;
    public Dictionary<int, CharExpInfo> charExpDataDic => _charExpDataDic;
    public Dictionary<int, SkillExpInfo> skillExpDataDic => _skillExpDataDic;
    public Dictionary<string, StringInfo> stringDataDic => _stringDataDic;

    public void LoadTableDatas()
    {
        LoadTable<int, CharInfo>("CharTable", out _charDataDic);
        LoadTable<int, MobInfo>("MobTable", out _mobDataDic);
        LoadTable<int, MapInfo>("MapTable", out _mapDataDic);
        foreach (var pair in mapDataDic) pair.Value.Setup();
        LoadTable<int, StageInfo>("StageTable", out _stageDataDic);
        foreach (var pair in stageDataDic) pair.Value.Setup();
        LoadTable<int, SkillInfo>("SkillTable", out _skillDataDic);
        LoadTable<int, SkillSetInfo>("SkillSetTable", out _skillSetDataDic);
        foreach (var pair in skillSetDataDic) pair.Value.Setup();
        LoadTable<int, RewardInfo>("RewardTable", out _rewardDataDic);
        foreach (var pair in rewardDataDic) pair.Value.Setup();
        LoadTable<int, CharExpInfo>("CharExpTable", out _charExpDataDic);
        LoadTable<int, SkillExpInfo>("SkillExpTable", out _skillExpDataDic);
        LoadTable<string, StringInfo>("StringTable", out _stringDataDic);
        foreach (var pair in stringDataDic) pair.Value.Setup();
    }

    private void LoadTable<Key, Value>(string jsonFileName, out Dictionary<Key, Value> dataDic) where Value : IData<Key>
    {
        TextAsset jsonText = Resources.Load<TextAsset>("DataTable/" + jsonFileName);
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

    public string GetString(string stringID)
    {
        if (stringDataDic.TryGetValue(stringID, out StringInfo info))
        {
            return info.message[PlayerData.language];
        }
        return "null";
    }
}
