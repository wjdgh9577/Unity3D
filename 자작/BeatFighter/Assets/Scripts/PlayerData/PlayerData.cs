using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public enum Language
{
    English,
    Korean
}

[System.Serializable]
public class DataFormat
{
    public int currentChar;
    public List<int> currentSkills;

    public int targetingParticle;
    public int HPParticle;
    public int MaxHPUI;
    public int NoteUI;

    public List<int> charDataDicKeys;
    public List<PlayerData.CharData> charDataDicValues;

    /// <summary>
    /// 세이브를 위한 데이터 포맷 변경
    /// </summary>
    /// <returns></returns>
    public static DataFormat GetCurrentData()
    {
        DataFormat data = new DataFormat();
        
        data.currentChar = PlayerData.currentChar;
        data.currentSkills = PlayerData.currentSkills;
        data.targetingParticle = PlayerData.targetingParticle;
        data.HPParticle = PlayerData.HPParticle;
        data.MaxHPUI = PlayerData.MaxHPUI;
        data.NoteUI = PlayerData.NoteUI;

        data.charDataDicKeys = new List<int>();
        data.charDataDicValues = new List<PlayerData.CharData>();
        foreach (var pair in PlayerData.charDataDic)
        {
            data.charDataDicKeys.Add(pair.Key);
            data.charDataDicValues.Add(pair.Value);
        }

        return data;
    }

    /// <summary>
    /// 현재 플레이어 데이터 갱신
    /// </summary>
    /// <param name="data"></param>
    public static void SetCurrentData(DataFormat data)
    {
        PlayerData.currentChar = data.currentChar;
        PlayerData.currentSkills = data.currentSkills;
        PlayerData.targetingParticle = data.targetingParticle;
        PlayerData.HPParticle = data.HPParticle;
        PlayerData.MaxHPUI = data.MaxHPUI;
        PlayerData.NoteUI = data.NoteUI;

        PlayerData.charDataDic = new Dictionary<int, PlayerData.CharData>();
        for (int i = 0; i < data.charDataDicKeys.Count; i++)
        {
            PlayerData.charDataDic.Add(data.charDataDicKeys[i], data.charDataDicValues[i]);
        }
    }
}

public static class PlayerData
{
    [System.Serializable]
    public struct CharData
    {
        public int level;
        public int exp;

        public List<int> skills;
        //장착 아이템 리스트 구현

        public void Initialize(int typeID)
        {
            level = 1;
            exp = 0;
            skills = TableData.instance.skillSetDataDic[typeID].skillIDs.GetRange(0, 4);
        }
    }

    public static Language language;

    public static int currentChar;
    public static List<int> currentSkills;

    public static int targetingParticle;
    public static int HPParticle;
    public static int MaxHPUI;
    public static int NoteUI;

    public static Dictionary<int, CharData> charDataDic;
    //public static Dictionary<int, ItemData> itemData; // 보유중인 모든 아이템

    /// <summary>
    /// 현재 캐릭터 변경
    /// </summary>
    /// <param name="typeID"></param>
    public static void ChangeCurrentChar(int typeID)
    {
        currentChar = typeID;
        currentSkills = charDataDic[currentChar].skills;
    }

    /// <summary>
    /// 새 계정 생성
    /// </summary>
    public static void NewAccountSetup()
    {
        currentChar = 20000;
        currentSkills = new List<int>() { 40000, 40001, 40002, 40003 };

        targetingParticle = 12000;
        HPParticle = 12100;
        MaxHPUI = 13000;
        NoteUI = 13100;

        CharData charData = new CharData();
        charData.Initialize(20000);
        CharData charData1 = new CharData();
        charData1.Initialize(20001);
        charDataDic = new Dictionary<int, CharData>() { { 20000, charData }, { 20001, charData1 } };
    }

    public static void SaveData()
    {
        if (File.Exists(GetPath()))
        {
            File.Delete(GetPath());
        }

        FileStream fs = new FileStream(GetPath(), FileMode.Create, FileAccess.Write);

        string data = JsonFx.Json.JsonWriter.Serialize(DataFormat.GetCurrentData());
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
        fs.Write(bytes, 0, bytes.Length);
        
        fs.Close();
    }

    public static bool LoadData()
    {
        if (!File.Exists(GetPath()))
        {
            return false;
        }

        FileStream fs = new FileStream(GetPath(), FileMode.Open, FileAccess.Read);

        byte[] bytes = new byte[(int)fs.Length];
        fs.Read(bytes, 0, (int)fs.Length);
        string data = System.Text.Encoding.UTF8.GetString(bytes);
        DataFormat formattedData = JsonUtility.FromJson<DataFormat>(data);
        DataFormat.SetCurrentData(formattedData);
        
        fs.Close();
        
        return true;
    }

    private static string GetPath()
    {
        return Path.Combine(Application.persistentDataPath, "save.bf");
    }

    public static void SetLanguage()
    {
        language = (Language)PlayerPrefs.GetInt("language");
    }
}
