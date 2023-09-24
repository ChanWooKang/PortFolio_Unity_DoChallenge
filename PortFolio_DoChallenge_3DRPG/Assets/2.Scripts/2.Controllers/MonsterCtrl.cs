using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Defines;

public class MonsterCtrl : FSM<MonsterCtrl>
{
    //Components
    public MonsterStat _stat;
    Animator _anim;
    Rigidbody _rb;
    CapsuleCollider _colider;
    Renderer[] _meshs;
    NavMeshAgent _agent;

    public MonsterType mType = MonsterType.Unknown;
    public SODropTable _dropTable;

    [HideInInspector] public Vector3 _offSet = Vector3.zero;
    [HideInInspector] public Vector3 _defPos = Vector3.zero;
    [HideInInspector] public Transform target = null;
    [HideInInspector] public Vector3 targetPos;
    [HideInInspector] public float lastCallTime;
    [HideInInspector] public float delayTime;
    [HideInInspector] public float cntTime;
    [HideInInspector] public bool isDead;
    [HideInInspector] public bool isAttack;
    [HideInInspector] public bool isReturnHome;

    MonsterState _nowState = MonsterState.Idle;
    ComboType _nowCombo = ComboType.Hit1;

    public NavMeshAgent Agent 
    { 
        get 
        { 
            if (_agent == null)
            {
                _agent = gameObject.GetOrAddComponent<NavMeshAgent>();
            }
            return _agent; 
        } 
    }

    public MonsterState State
    {
        get { return _nowState; }
        set
        {
            _nowState = value;
            ChangeAnim(_nowState);
        }
    }

    public ComboType nowCombo { get { return _nowCombo; } set { _nowCombo = value; } }
    
    public PlayerCtrl player { get { return PlayerCtrl._inst; } }


    void Start()
    {
        InitComponents();
        State = MonsterState.Idle;
    }

    void Update()
    {
        FSMUpdate();
    }

    void FixedUpdate()
    {
        FreezeRotation();
    }

    void InitComponents()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _colider = GetComponent<CapsuleCollider>();
        _meshs = GetComponentsInChildren<Renderer>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        if (mType != MonsterType.Unknown && mType != MonsterType.Max_Cnt)
        {
            _stat = GetComponent<MonsterStat>();
            _stat.SetStat(mType);
        }
        else
        {
            Debug.Log($"몬스터 타입 설정이 안되어 있습니다 {gameObject.name}");
        }
    }

    public void InitDatas()
    {
        targetPos = Vector3.zero;
        lastCallTime = 0;
        delayTime = 2.0f;
        isDead = false;
        isAttack = false;
        isReturnHome = false;
    }

    public void InitTarget()
    {
        if (player != null && player.State != PlayerState.Die)
            target = player.transform;
        else
            target = null;
    }

    void FreezeRotation()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    public void ChangeAnim(MonsterState type)
    {
        switch(type)
        {
            case MonsterState.Die:
                _anim.CrossFade("Die",0.1f);
                break;
            case MonsterState.Idle:
                _anim.CrossFade("Idle", 0.1f);
                break;
            case MonsterState.Sense:
                _anim.CrossFade("Sense", 0.1f, -1, 0);
                break;
            case MonsterState.Patrol:
                _anim.CrossFade("Patrol", 0.1f);
                break;
            case MonsterState.Trace:
                _anim.CrossFade("Trace", 0.1f);
                break;
            case MonsterState.Attack:
                switch (mType)
                {
                    default:
                        {
                            switch (nowCombo)
                            {
                                case ComboType.Hit1:
                                    _anim.CrossFade("Hit1", 0.1f, -1, 0);
                                    nowCombo = ComboType.Hit2;
                                    break;
                                case ComboType.Hit2:
                                    _anim.CrossFade("Hit2", 0.1f, -1, 0);
                                    nowCombo = ComboType.Hit1;
                                    break;
                            }
                        }
                        break;
                }
                break;
        }
    }

    void ChangeColor(Color color)
    {
        if(_meshs.Length > 0)
        {
            foreach (Renderer mesh in _meshs)
                mesh.material.color = color;
        }
    }

    public void ChangeLayer(LayerType layer)
    {
        int i = (int)layer;

        if (gameObject.layer != i)
            gameObject.layer = i;
    }

    public Vector3 GetRandomPos(float range = 5.0f)
    {
        Vector3 pos = Random.onUnitSphere;
        pos.y = 0;
        float r = Random.Range(0, range);
        pos = _defPos + (pos * r);

        NavMeshPath path = new NavMeshPath();
        if (_agent.CalculatePath(pos, path))
            return pos;
        else
            return GetRandomPos();
    }

    public bool IsCloseTarget(Vector3 pos, float range)
    {
        float dist = Vector3.SqrMagnitude(transform.position - pos);
        if (dist < range * range)
            return true;
        return false;
    }

    public void MoveFunc(Vector3 pos)
    {
        Vector3 dir = pos - transform.position;
        _agent.SetDestination(pos);
        _agent.speed = _stat.MoveSpeed;
        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(dir), 20 * Time.deltaTime);
    }

    public void TurnTowardPlayer()
    {
        if(player != null)
        {
            Vector3 dir = player.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    public void AttackFunc()
    {
        if(target ==null || player.State == PlayerState.Die)
        {
            if (target != null)
                target = null;
            ChangeState(MonsterStatePatrol._inst);
            return;
        }

        State = MonsterState.Attack;
        isAttack = true;
    }

    public void OnAttackEvent()
    {
        if(target != null && player.State == PlayerState.Die)
        {
            if(IsCloseTarget(target.position, _stat.AttackRange))
            {
                if (player.OnDamage(_stat))
                {
                    target = null;
                    ChangeState(MonsterStatePatrol._inst);
                }
            }
        }
    }
    public void OffAttackEvent()
    {
        isAttack = false;
        State = MonsterState.Trace;
    }

    public bool OnDamage(BaseStat stat)
    {
        if (isDead)
            return true;

        isDead = _stat.GetHit(stat);
        StopCoroutine(OnDamageEvent());
        StartCoroutine(OnDamageEvent());

        return isDead;
    }

    IEnumerator OnDamageEvent()
    {
        if (isDead)
        {
            _colider.enabled = false;
            _agent.SetDestination(transform.position);
            _stat.DeadFunc(player._stat);
            ChangeColor(Color.gray);
            ChangeState(MonsterStateDie._inst);
            yield break;
        }
        ChangeColor(Color.red);
        yield return new WaitForSeconds(0.3f);
        ChangeColor(Color.white);   
    }

    public void OnDeadEvent()
    {
        SpawnManager._inst.MonsterDespawn(gameObject);
        ChangeColor(Color.white);
        _dropTable.ItemDrop(transform, _stat.Gold);
        _dropTable.ItemDrop(transform);
        ChangeState(MonsterStateDisable._inst);
    }

    public void OnResurrectEvent()
    {
        if(_colider.enabled == false)
            _colider.enabled = true;
        ChangeLayer(LayerType.Monster);
        State = MonsterState.Idle;
    }
}
