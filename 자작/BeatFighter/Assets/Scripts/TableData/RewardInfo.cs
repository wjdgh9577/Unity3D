using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardInfo : TableData.IData<int>
{
    public int typeID;

    public int exp;
    public int gold;

    public int item0;
    public int percent0;
    public int item1;
    public int percent1;
    public int item2;
    public int percent2;
    public int character;
    public int percent3;

    public Dictionary<string, Dictionary<int, int>> rewards;

    public int Key()
    {
        return typeID;
    }

    public void Setup()
    {
        rewards = new Dictionary<string, Dictionary<int, int>>();
        rewards.Add("character", new Dictionary<int, int>());
        rewards.Add("item", new Dictionary<int, int>());

        Add("item", item0, percent0);
        Add("item", item1, percent1);
        Add("item", item2, percent2);
        Add("character", character, percent3);
    }

    private void Add(string key, int item, int percent)
    {
        if (percent != 0) rewards[key].Add(item, percent);
    }
}
