using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

[System.Serializable]
public class StatByLevel
{
    public int level;
    public float hp;
    public float mp;
    public float damage;
    public float defense;
    public float exp;

    public StatByLevel()
    {
        level = 1;
        hp = 150;
        mp = 150;
        damage = 50;
        defense = 20;
        exp = 500;
    }
}

[System.Serializable]
public class StatData : ILoader<int, StatByLevel>
{
    public List<StatByLevel> stats = new List<StatByLevel>();

    public Dictionary<int, StatByLevel> Make()
    {
        Dictionary<int, StatByLevel> dict = new Dictionary<int, StatByLevel>();
        foreach(StatByLevel stat in stats)
        {
            dict.Add(stat.level, stat);
        }
        return dict;
    }

    public StatData() { }
}


