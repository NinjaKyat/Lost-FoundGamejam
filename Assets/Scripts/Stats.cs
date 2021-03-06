using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{
    Dictionary<string, int> internalStats = new Dictionary<string, int>();
    public const string healthStat = "health";
    public const string moveSpeedStat = "movementSpeed";
    public const string foodStat = "food";
    public const string attackStat = "attack";
    public const string randomStat = "random";

    public Dictionary<string,int> GetDictionary()
    {
        return internalStats;
    }

    public int GetStat(string stat)
    {
        if (stat == randomStat)
        {
            return Random.Range(0, 100);
        }
        if (internalStats.ContainsKey(stat))
        {
            return internalStats[stat];
        }
        return 0;
    }

    public int GetMaxStat(string stat)
    {
        var maxStatKey = stat + "Max";
        return GetStat(maxStatKey);
    }

    public void AddStat(string stat, int amount)
    {
        if (internalStats.ContainsKey(stat))
        {
            internalStats[stat] += amount;
        }
        else
        {
            internalStats[stat] = amount;
        }
    }

    public void SetStat(string stat, int value)
    {
        internalStats[stat] = value;
    }
}
