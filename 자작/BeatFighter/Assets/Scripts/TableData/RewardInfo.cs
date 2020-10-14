using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardInfo : TableData.IData<int>
{
    public int typeID;

    public int exp;
    public int gold;

    public int item0;
    public float percent0;
    public int item1;
    public float percent1;
    public int item2;
    public float percent2;
    public int character;
    public float percent3;

    private Dictionary<string, Dictionary<int, float>> rewards;

    public int Key()
    {
        return typeID;
    }

    public void Setup()
    {
        rewards = new Dictionary<string, Dictionary<int, float>>();
        rewards.Add("character", new Dictionary<int, float>());
        rewards.Add("item", new Dictionary<int, float>());

        Add("item", item0, percent0);
        Add("item", item1, percent1);
        Add("item", item2, percent2);
        Add("character", character, percent3);
    }

    public Dictionary<string, List<int>> GetReward()
    {
        Dictionary<string, List<int>> reward = new Dictionary<string, List<int>>();
        reward.Add("character", new List<int>());
        reward.Add("item", new List<int>());

        foreach (var pair1 in rewards)
        {
            if (pair1.Value.Count == 0) continue;
            foreach (var pair2 in rewards[pair1.Key])
            {
                float percent = UnityEngine.Random.Range(0, 100);
                if (percent <= pair2.Value) reward[pair1.Key].Add(pair2.Key);
            }
        }

        return reward;
    }

    private void Add(string key, int item, float percent)
    {
        if (percent != 0) rewards[key].Add(item, percent);
    }
}
