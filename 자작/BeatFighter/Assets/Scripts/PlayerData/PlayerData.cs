using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public static int level = 5; // for test

    public static int currentChar = 10000;
    public static List<int> currentSkills = new List<int>() { 50000, 50000, 50000, 50000 };

    public static int targetingParticle = 2000;
    public static int HPParticle = 2100;
    public static int MaxHPUI = 3000;
    public static int NoteUI = 3100;

    public static Dictionary<int, CharData> charData;
    public static Dictionary<int, ItemData> itemData;
}
