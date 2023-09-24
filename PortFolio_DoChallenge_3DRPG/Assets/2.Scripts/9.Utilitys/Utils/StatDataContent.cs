using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

namespace Data_Stat
{
    #region [Player]
    [System.Serializable]
    public class StatByLevel
    {
        public int level;
        public float hp;
        public float mp;
        public float damage;
        public float defense;
        public float exp;
    }
    [System.Serializable]
    public class SavePlayerStat
    {
        public int level;
        public float hp;
        public float mp;
        public float exp;
        public int gold;
    }

    [System.Serializable]
    public class PlusStat
    {
        public float hp;
        public float mp;
        public float damage;
        public float defense;
    }
    #endregion [Player]
    #region [Monster]
    [System.Serializable]
    public class StatByMonster
    {
        public int index;
        public float hp;
        public float damage;
        public float defense;
        public float movespeed;
        public float tracerange;
        public float attackrange;
        public float attackdelay;
        public int mingold;
        public int maxgold;
        public float exp;
    }
    #endregion [Monster]

    [System.Serializable]
    public class StatData : ILoader<int, StatByLevel>
    {
        public List<StatByLevel> stats = new List<StatByLevel>();

        public Dictionary<int, StatByLevel> Make()
        {
            Dictionary<int, StatByLevel> dict = new Dictionary<int, StatByLevel>();
            foreach (StatByLevel stat in stats)
            {
                dict.Add(stat.level, stat);
            }
            return dict;
        }

        public StatData() { }
    }

    [System.Serializable]
    public class MonsterData : ILoader<int, StatByMonster>
    {
        public List<StatByMonster> stats = new List<StatByMonster>();

        public Dictionary<int, StatByMonster> Make()
        {
            Dictionary<int, StatByMonster> dict = new Dictionary<int, StatByMonster>();
            foreach (StatByMonster stat in stats)
            {
                dict.Add(stat.index, stat);
            }
            return dict;
        }

        public MonsterData() { }
    }

}

