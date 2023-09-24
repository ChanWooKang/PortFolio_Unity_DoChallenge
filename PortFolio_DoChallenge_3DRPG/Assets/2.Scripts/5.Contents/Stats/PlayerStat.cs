using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;
using Data_Stat;

public class PlayerStat : BaseStat
{
    [SerializeField]
    protected float _mp;
    [SerializeField]
    protected float _maxmp;
    [SerializeField]
    protected float _exp;
    [SerializeField]
    protected float _totalexp;
    [SerializeField]
    protected int _gold;
    [SerializeField]
    protected float _plushp;
    [SerializeField]
    protected float _plusmp;
    [SerializeField]
    protected float _plusdamage;
    [SerializeField]
    protected float _plusdefense;
    
    public float MP { get { return _mp; } set { _mp = value; } }
    public float MaxMP { get { return _maxmp; } }
    public float Exp 
    {
        get { return _exp; } 
        set 
        {
            _exp = value;
            int level = 1;
            while (true)
            {
                if (Managers._data.StatDict.TryGetValue(level + 1, out StatByLevel stat) == false)
                    break;
                if (_exp < stat.exp) 
                        break;
                level++;
            }

            if(level != _level)
            {
                _level = level;
                SetStat(_level);
            }
        } 
    }

    public float TotalExp { get { return _totalexp; } }

    public int Gold { get { return _gold; } set { _gold = value; } }

    public float PlusHP { get { return _plushp; } set { _plushp = value;} }
    public float PlusMP { get { return _plusmp; } set { _plusmp = value;} }
    public float PlusDamage { get { return _plusdamage; } set { _plusdamage = value;} }
    public float PlusDefense { get { return _plusdefense; } set { _plusdefense = value;} }

    void Start()
    {
        LoadPlayerData();
        LoadPlusData();
        InventoryManager._inst.OnChangeStat?.Invoke();
    }

    void Init()
    {
        _level = 1;
        _maxhp = 200;
        _maxmp = 200;
        _damage = 50;
        _defense = 5;
        _moveSpeed = 10;
        _exp = 0;
        _gold = 0;
        _hp = _maxhp;
        _mp = _maxmp;
    }

    public float EXP()
    {
        if(_level == 1)
        {
            return _exp;
        }
        else
        {
            if(Managers._data.StatDict.TryGetValue(_level-1, out StatByLevel stat))
            {
                return _exp - stat.exp;
            }
            else
            {
                return -1;
            }
        }
    }


    public float TotalEXP()
    {
        if (Managers._data.StatDict.TryGetValue(_level, out StatByLevel stat) == false)
        {
            return -1;
        }
        else
        {
            if (Managers._data.StatDict.TryGetValue(_level + 1, out StatByLevel Nextstat) == false)
            {
                return -1;
            }
            else
            {
                return Nextstat.exp - stat.exp;
            }
            
        }
    }

    // 레벨 업 했을 경우 활용
    public void SetStat(int level)
    {
        Dictionary<int, StatByLevel> dict = Managers._data.StatDict;
        StatByLevel stat = dict[level];
        _hp = stat.hp;
        _maxhp = stat.hp;
        _mp = stat.mp;
        _maxmp = stat.mp;
        _damage = stat.damage;
        _defense = stat.defense;
    }

    public void LoadPlayerData()
    {
        string data = Managers._file.LoadJsonFile("PlayerData");
        if (string.IsNullOrEmpty(data) == false)
        {
            SavePlayerStat stat = JsonUtility.FromJson<SavePlayerStat>(data);
            _level = stat.level;
            _hp = stat.hp;
            _mp = stat.mp;
            _exp = stat.exp;
            _gold = stat.gold;
            _moveSpeed = 10;
            StatByLevel SBL;
            if(Managers._data.StatDict.TryGetValue(_level, out SBL))
            {
                _maxhp = SBL.hp;
                _maxmp = SBL.mp;
                _damage = SBL.damage;
                _defense = SBL.defense;
            }
            else
            {
                Debug.Log("Failed To Load PlayerStat By StatByLevel");
                Init();
            }

        }
        else
        {
            Debug.Log("Failed To Load PlayerStat By SavePlayerStat");
            Init();
        }
    }

    public void LoadPlusData()
    {
        string data = Managers._file.LoadJsonFile("PlusData");
        if (string.IsNullOrEmpty(data) == false)
        {
            //데이터가 있을경우
            PlusStat stat = JsonUtility.FromJson<PlusStat>(data);
            SetPlusData(stat.hp, stat.mp, stat.damage, stat.defense);
            SetMaxData();
        }
        else
        {
            //데이터가 없을 경우
            SetPlusData(0, 0, 0, 0);
            Debug.Log("Failed To Load PlayerStat By PlusData");
        }
        
    }

    public void SavePlayerData()
    {
        SavePlayerStat stat = new SavePlayerStat
        {
            level = _level,
            hp = _hp,
            mp = _mp,
            exp = _exp,
            gold = _gold
        };
        Managers._file.SaveJsonFile<SavePlayerStat>(stat, "PlayerData");
    }

    public void SavePlusData()
    {
        PlusStat stat = new PlusStat
        {
            hp = _plushp,
            mp = _plusmp,
            damage = _plusdamage,
            defense = _plusdefense
        };
        Managers._file.SaveJsonFile<PlusStat>(stat, "PlusData");
    }

    void SetPlusData(float hp, float mp, float dam, float def)
    {
        _plushp = hp;
        _plusmp = mp;
        _plusdamage = dam;
        _plusdefense = def;
    }

    void SetMaxData()
    {
        _maxhp += _plushp;
        _maxmp += _plusmp;
        _damage += _plusdamage;
        _defense += _plusdefense;
    }

    public void AddPlusStat(StatType type, float value)
    {
        switch(type)
        {
            case StatType.HP:
                PlusHP += value;
                _hp = Mathf.Min(_hp, _maxhp);
                _maxhp += value;
                break;
            case StatType.MP:
                PlusMP += value;
                _mp = Mathf.Min(_mp, _maxhp);
                _maxmp += value;
                break;
            case StatType.Damage:
                PlusDamage += value;
                _damage += value;
                break;
            case StatType.Defense:
                PlusDefense += value;
                _defense += value;
                break;
        }

        if(value < 0)
        {
            if(_hp > _maxhp)
            {
                
            }
            
            if(_mp > MaxMP)
            {

            }
        }
    }

    public void UsePotion(StatType type, float value)
    {
        switch (type)
        {
            case StatType.HP:
                _hp = Mathf.Min(_hp + value, _maxhp);
                break;
            case StatType.MP:
                _mp = Mathf.Min(_mp + value, _maxhp);
                break;
        }
    }

    public override bool GetHit(BaseStat attacker)
    {
        return base.GetHit(attacker);
    }

    void OnApplicationQuit()
    {
        SavePlayerData();
        SavePlusData();
    }
}
