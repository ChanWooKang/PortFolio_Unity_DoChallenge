using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStat : MonoBehaviour
{
    [SerializeField]
    protected int _level;
    [SerializeField]
    protected float _hp;
    [SerializeField] 
    protected float _maxhp;
    [SerializeField]
    protected float _damage;
    [SerializeField]
    protected float _defense;
    [SerializeField]
    protected float _moveSpeed;
    
    public int Level { get { return _level; } set { _level = value; } }
    public float HP { get { return _hp; } set { _hp = value; } }
    public float MaxHP {  get { return _maxhp; } set { _maxhp = value; } }
    public float Damage { get { return _damage; } set { _damage = value; } }
    public float Defense { get { return _defense; } set { _defense = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }


    public virtual bool GetHit(BaseStat attacker)
    {
        float damage = Mathf.Max(0, attacker.Damage - _defense);
        if (_hp > damage)
        {
            _hp -= damage;
            return false;
        }
        else
        {
            _hp = 0;
            return true;
        }
    }
}
