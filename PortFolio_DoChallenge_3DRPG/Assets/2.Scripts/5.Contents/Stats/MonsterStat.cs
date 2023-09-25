using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;
using Data_Stat;

public class MonsterStat : BaseStat
{
    [SerializeField]
    protected MonsterType _type;
    [SerializeField]
    protected float _tracerange;
    [SerializeField]
    protected float _traceSpeed;
    [SerializeField]
    protected float _attackrange;
    [SerializeField]
    protected float _attackDelay;
    [SerializeField]
    protected int _mingold;
    [SerializeField]
    protected int _maxgold;
    [SerializeField]
    protected float _exp;

    public MonsterType Type {  get { return _type; } }
    public float TraceRange { get { return _tracerange; } }
    public float TraceSpeed { get { return _traceSpeed; } }
    public float AttackRange { get { return _attackrange; } }
    public float AttackDelay { get { return _attackDelay; } }
    public int Gold { get { return Random.Range(_mingold, _maxgold); } }
    public float Exp { get { return _exp; } }
    
    void Init()
    {
        _type = MonsterType.Unknown;
        _tracerange = 0;
        _attackrange = 0;
        _attackDelay = 0;
        _mingold = 0;
        _maxgold = 0;
        _exp = 0;
    }

    public void SetStat(MonsterType type)
    {
        int index = (int)type;
        if(Managers._data.MonsterDict.TryGetValue(index, out StatByMonster stat))
        {
            _type = type;
            _hp = stat.hp;
            _maxhp = stat.hp;
            _damage = stat.damage;
            _defense = stat.defense;
            _moveSpeed = 6;
            _traceSpeed = stat.movespeed;
            _tracerange = stat.tracerange;
            _attackrange = stat.attackrange;
            _attackDelay = stat.attackdelay;
            _mingold = stat.mingold;
            _maxgold = stat.maxgold;
            _exp = stat.exp;
            
        }
        else
        {
            Init();
        }
    }

    public override bool GetHit(BaseStat attacker)
    {
       return base.GetHit(attacker);
    }

    public void DeadFunc(PlayerStat stat)
    {
        if(stat != null)
        {
            stat.Exp += _exp;
        }
    }
}

