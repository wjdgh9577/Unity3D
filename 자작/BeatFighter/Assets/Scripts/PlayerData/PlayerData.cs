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
    public int tutorial_combat;
    public int tutorial_menu;
    public int tutorial_collection;
    public int tutorial_skillmode;
    public int tutorial_itemmode;
    public int tutorial_journeymode;

    public int currentChar;
    public List<int> currentSkills;

    public int targetingParticle;
    public int HPParticle;
    public int MaxHPUI;
    public int NoteUI;

    public List<int> charDataDicKeys;
    public List<PlayerData.CharData> charDataDicValues;

    public List<int> completedMaps;

    /// <summary>
    /// 세이브를 위한 데이터 포맷 변경
    /// </summary>
    /// <returns></returns>
    public static DataFormat GetCurrentData()
    {
        DataFormat data = new DataFormat();

        data.tutorial_combat = PlayerData.tutorial_combat;
        data.tutorial_menu = PlayerData.tutorial_menu;
        data.tutorial_collection = PlayerData.tutorial_collection;
        data.tutorial_skillmode = PlayerData.tutorial_skillmode;
        data.tutorial_itemmode = PlayerData.tutorial_itemmode;
        data.tutorial_journeymode = PlayerData.tutorial_journeymode;

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

        data.completedMaps = PlayerData.completedMaps;

        return data;
    }

    /// <summary>
    /// 현재 플레이어 데이터 갱신
    /// </summary>
    /// <param name="data"></param>
    public static void SetCurrentData(DataFormat data)
    {
        PlayerData.tutorial_combat = data.tutorial_combat;
        PlayerData.tutorial_menu = data.tutorial_menu;
        PlayerData.tutorial_collection = data.tutorial_collection;
        PlayerData.tutorial_skillmode = data.tutorial_skillmode;
        PlayerData.tutorial_itemmode = data.tutorial_itemmode;
        PlayerData.tutorial_journeymode = data.tutorial_journeymode;

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

        PlayerData.completedMaps = data.completedMaps;
    }
}

public static class PlayerData
{
    [System.Serializable]
    public struct CharData
    {
        public int level;
        public int exp;

        public List<int> currentSkills;
        public List<int> skills;
        public List<SkillData> skillDatas;
        //장착 아이템 리스트 구현

        public void Initialize(int typeID)
        {
            level = 1;
            exp = 0;
            currentSkills = TableData.instance.skillSetDataDic[typeID].skillIDs.GetRange(0, 4);
            skills = TableData.instance.skillSetDataDic[typeID].skillIDs;
            skillDatas = new List<SkillData>();
            foreach (var skill in skills)
            {
                SkillData skillData = new SkillData();
                skillData.initialize();
                skillDatas.Add(skillData);
            }
        }

        public void SetData(int level, int exp)
        {
            this.level = level;
            this.exp = exp;
        }
    }

    [System.Serializable]
    public struct SkillData
    {
        public int level;
        public int exp;

        public void initialize()
        {
            level = 1;
            exp = 0;
        }

        public void SetData(int level, int exp)
        {
            this.level = level;
            this.exp = exp;
        }
    }

    // device setting
    public static Language language;
    public static float musicSoundDegree;
    public static float effectSoundDegree;

    // tutorial
    public static int tutorial_combat;
    public static int tutorial_menu;
    public static int tutorial_collection;
    public static int tutorial_skillmode;
    public static int tutorial_itemmode;
    public static int tutorial_journeymode;

    // selected character
    public static int currentChar;
    public static List<int> currentSkills;

    // favorite
    public static int targetingParticle;
    public static int HPParticle;
    public static int MaxHPUI;
    public static int NoteUI;

    // data dictionary
    public static Dictionary<int, CharData> charDataDic;
    //public static Dictionary<int, ItemData> itemData; // 보유중인 모든 아이템

    public static List<int> completedMaps;

    /// <summary>
    /// 현재 캐릭터 변경
    /// </summary>
    /// <param name="typeID"></param>
    public static void ChangeCurrentChar(int typeID)
    {
        currentChar = typeID;
        currentSkills = charDataDic[currentChar].currentSkills;
    }

    /// <summary>
    /// 새 계정 생성
    /// </summary>
    public static void NewAccountSetup()
    {
        tutorial_combat = 0;
        tutorial_menu = 0;
        tutorial_collection = 0;
        tutorial_skillmode = 0;
        tutorial_itemmode = 0;
        tutorial_journeymode = 0;

        currentChar = 20000;
        currentSkills = new List<int>() { 40000, 40001, 40002, 40003 };

        targetingParticle = 12000;
        HPParticle = 12100;
        MaxHPUI = 13000;
        NoteUI = 13100;

        CharData charData = new CharData();
        charData.Initialize(20000);
        charDataDic = new Dictionary<int, CharData>() { { 20000, charData } };

        completedMaps = new List<int>();
    }

    /// <summary>
    /// 현재 캐릭터의 스킬 데이터를 반환
    /// </summary>
    /// <param name="skillID"></param>
    /// <returns></returns>
    public static SkillData GetSkillData(int skillID)
    {
        int index = charDataDic[currentChar].skills.FindIndex(id => id == skillID);
        return charDataDic[currentChar].skillDatas[index];
    }

    public static void CompleteMap(int mapID)
    {
        if (completedMaps.Contains(mapID)) return;
        completedMaps.Add(mapID);
    }

    #region Save / Load
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
    #endregion

    #region Device Setting
    public static void SetLanguage()
    {
        language = (Language)PlayerPrefs.GetInt("language");
    }

    public static void SetMusicSoundDegree()
    {
        if (PlayerPrefs.HasKey("musicSoundDegree")) musicSoundDegree = PlayerPrefs.GetFloat("musicSoundDegree");
        else musicSoundDegree = 1;
    }

    public static void SetEffectSoundDegree()
    {
        if (PlayerPrefs.HasKey("effectSoundDegree")) effectSoundDegree = PlayerPrefs.GetFloat("effectSoundDegree");
        else effectSoundDegree = 1;
    }
    #endregion
}
