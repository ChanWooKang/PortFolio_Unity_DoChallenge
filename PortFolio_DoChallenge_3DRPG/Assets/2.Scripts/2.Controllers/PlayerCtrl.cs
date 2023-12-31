using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Defines;


public class PlayerCtrl : MonoBehaviour
{
    static PlayerCtrl instance;
   
    public PlayerStat _stat;
    //public WeaponCtrl _weapon;

    Animator _anim;
    Rigidbody _rb;
    NavMeshAgent _agent;
    Renderer[] _meshs;

    int _clickMask;
    int _blockMask;
    bool isStopAttack;
    bool isDead;

    public bool ActiveDodge;

    Vector3 _destPos;
    GameObject _locktarget;
    [SerializeField]
    GameObject _nearObj;

    InteractType _nearType;
    PlayerState _state = PlayerState.Idle;
    SkillType _sType = SkillType.Unknown;

    Vector3 mousePoint;

    public static PlayerCtrl _inst { get { return instance; } }
    public PlayerState State
    {
        get { return _state; }
        set
        {
            _state = value;
            if(_anim == null)
                _anim = GetComponent<Animator>();
            switch(_state)
            {
                case PlayerState.Die:
                    _anim.CrossFade("Die", 0.1f);
                    break;
                case PlayerState.Idle:
                    _anim.CrossFade("Idle", 0.1f);
                    break;
                case PlayerState.Move:
                    _anim.CrossFade("Move", 0.1f);
                    break;
                case PlayerState.Attack:
                    _anim.CrossFade("Attack", 0.1f, -1, 0);
                    break;
                case PlayerState.Skill:
                    {
                        switch (_sType)
                        {
                            case SkillType.Dodge:
                                _anim.CrossFade("Dodge", 0.1f, -1, 0);
                                break;
                            case SkillType.Slash:
                                _anim.CrossFade("Slash", 0.1f, -1, 0);
                                break;
                        }
                    }
                    break;
            }
        }
    }
    public GameObject LockTarget { get { return _locktarget; } set { _locktarget = value; } }

    void Awake()
    {
        InitComponents();
    }

    void Start()
    {
        InitData();
        Managers._input.KeyAction -= OnKeyBoardEvent;
        Managers._input.KeyAction += OnKeyBoardEvent;
        Managers._input.RightAction -= OnMouseEvent;
        Managers._input.RightAction += OnMouseEvent;
    }

    void Update()
    {
        switch(_state)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Move:
                UpdateMove();
                break;
            case PlayerState.Attack:
                UpdateAttack();
                break;
            case PlayerState.Skill:
                {
                    switch (_sType)
                    {
                        case SkillType.Dodge:
                            UpdateDodge();
                            break;
                    }
                }
                break;
        }
    }

    void FixedUpdate()
    {
        switch (_sType)
        {
            default:
                FreezeRotate();
                break;
            case SkillType.Dodge:
                break;
        }
    }

    void InitComponents()
    {
        instance = this;
        _stat = GetComponent<PlayerStat>();
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
        _meshs = GetComponentsInChildren<Renderer>();
    }

    void InitData()
    {
        //터치 이벤트 적용할 레이어 ( Floor, Monster )
        _clickMask = (1 << (int)LayerType.Floor) + (1 << (int)LayerType.Monster);
        // 이동 불가 레이어 인식
        _blockMask = (1 << (int)LayerType.Block);
        isStopAttack = true;
        isDead = false;

        _nearObj = null;
        _locktarget = null;
        _nearType = InteractType.Unknown;
        _destPos = transform.position;
        mousePoint = transform.position;
        ActiveDodge = false;
    }

    void FreezeRotate()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    void ChangeColor(Color color)
    {
        foreach(Renderer mesh in _meshs)
            mesh.material.color = color;
    }

    void CheckAttackable(float range = 2.0f)
    {
        if (_locktarget == null)
            return;

        _destPos = _locktarget.transform.position;
        float dist = Vector3.SqrMagnitude(_destPos - transform.position);
        if(dist < range * range)
            State = PlayerState.Attack;

        return;
    }

    void OnKeyBoardEvent()
    {
        if (isDead || UI_Inventory.ActivatedInventory)
            return;

        OnInteractEvent(Input.GetKeyDown(KeyCode.F));
    }

    void OnInteractEvent(bool btnDown = false)
    {
        if (_nearObj == null || _nearType == InteractType.Unknown)
            return;

        if (ActiveDodge == true)
            return;

        if (btnDown)
        {
            switch (_nearType)
            {
                case InteractType.RootItem:
                    {
                        if(_nearObj.TryGetComponent<Item>(out Item item))
                        {
                            if(item.Root())
                                ClearNearObject();
                        }
                    }
                    break;
            }
        }
    }

    void OnMouseEvent(MouseEvent evt)
    {
        if (isDead || UI_Inventory.ActivatedInventory)
            return;

        if (ActiveDodge == true)
            return;


        switch (_state)
        {
            case PlayerState.Idle:
                OnMouseEvent_IDLEMOVE(evt);
                break;
            case PlayerState.Move:
                OnMouseEvent_IDLEMOVE(evt);
                break;
            case PlayerState.Attack:
                {
                    if (evt == MouseEvent.PointerUp)
                        isStopAttack = true;
                }
                break;
        }
    }

    void OnMouseEvent_IDLEMOVE(MouseEvent evt)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool isHit = Physics.Raycast(ray, out RaycastHit rhit, 100, _clickMask);
        switch (evt)
        {
            case MouseEvent.PointerDown:
                {
                    if (isHit)
                    {
                        _destPos = rhit.point;
                        if(State != PlayerState.Move)
                            State = PlayerState.Move;
                        isStopAttack = false;
                        if (rhit.collider.gameObject.layer == (int)LayerType.Monster)
                            _locktarget = rhit.collider.gameObject;
                        else
                            _locktarget = null;
                    }
                }
                break;
            case MouseEvent.Press:
                {
                    if (_locktarget == null && isHit)
                        _destPos = rhit.point;
                }
                break;
            case MouseEvent.PointerUp:
                isStopAttack = true;
                break;
        }
    }

    void UpdateMove()
    {
        CheckAttackable();
        Vector3 dir = _destPos - transform.position;
        dir.y = 0;
        if (dir.sqrMagnitude < 0.01f)
            State = PlayerState.Idle;
        else
        {
            float dist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.sqrMagnitude);
            _agent.Move(dir.normalized * dist);

            if(Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, _blockMask))
            {
                if (Input.GetMouseButton(0))
                    return;
            }

            if (dir != Vector3.zero)
            {
                Quaternion rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, 20.0f * Time.deltaTime);
            }
                
        }
    }

    void UpdateAttack()
    {
        if(_locktarget != null)
        {
            Vector3 dir = _locktarget.transform.position - transform.position;
            if(dir != Vector3.zero)
            {
                Quaternion rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Lerp(transform.rotation, rot, 20.0f * Time.deltaTime);
            }
        }
    }

    void UpdateDodge()
    {
        if (mousePoint != transform.position)
        {
            if(mousePoint != Vector3.zero)
            {
                Quaternion rot = Quaternion.LookRotation(mousePoint);
                transform.rotation = Quaternion.Lerp(transform.rotation, rot, 20.0f * Time.deltaTime);
            }
        }
    }

    public void OnAttackEvent()
    {
        if(_locktarget != null)
        {
            if(_locktarget.TryGetComponent<MonsterCtrl>(out MonsterCtrl mc))
            {
                mc.OnDamage(_stat);
                if (mc.isDead)
                {
                    _locktarget = null;
                    isStopAttack = true;
                }
            }
        }

       
    }

    public void OffAttackEvent()
    {
        if (isStopAttack)
            State = PlayerState.Idle;
        else
            State = PlayerState.Attack;
    }

    public void OnDamage(BaseStat stat)
    {
        if (isDead)
            return;

        isDead = _stat.GetHit(stat);
        StopCoroutine(OnDamageEvent());
        StartCoroutine(OnDamageEvent());
    }

    IEnumerator OnDamageEvent()
    {
        if (isDead)
        {
            State = PlayerState.Die;
            ChangeColor(Color.gray);
            yield return new WaitForSeconds(2.0f);
            OnDeadEvent();
            yield break;
        }
        ChangeColor(Color.red);
        yield return new WaitForSeconds(0.3f);
        ChangeColor(Color.white);
    }

    void OnDeadEvent()
    {
        gameObject.DestroyAPS();
        ChangeColor(Color.white);
    }

    public void ClearNearObject()
    {
        _nearObj = null;
        _nearType = InteractType.Unknown;
    }

    public void EarnMoney(int gold)
    {
        _stat.Gold += gold;
    }
    
    public void AddPlusStat(StatType type, float amount)
    {
        _stat.AddPlusStat(type, amount);
    }
    public void UsePotion(StatType type, float value)
    {
        _stat.UsePotion(type, value);
    }

    public void SkillEffect(SkillType type, SOSkill sSkill)
    {
        switch (type)
        {
            case SkillType.Heal:
                HealEffect(sSkill);
                break;
            case SkillType.Dodge:
                DodgeEffect(sSkill);
                break;
            case SkillType.Slash:
                SlashEffect(sSkill);
                break;
        }
    }

    bool CheckMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit rhit, 100.0f, (1 << (int)LayerType.Floor)))
        {
            mousePoint = rhit.point - transform.position;
            mousePoint.y = 0;
            mousePoint = mousePoint.normalized;
            return true;
        }

        return false;
    }

    void HealEffect(SOSkill sSkill)
    {
       
        float value = _stat.MaxHP * sSkill.effectValue;
        UsePotion(StatType.HP, value);
        StopCoroutine(OnHealEvent(sSkill.skillName));
        StartCoroutine(OnHealEvent(sSkill.skillName));
        
    }

    void SlashEffect(SOSkill sSkill)
    {
        if(CheckMousePosition())
        {
            StopCoroutine(OnSlashEvent(sSkill));
            StartCoroutine(OnSlashEvent(sSkill));
        }
    }

    void DodgeEffect(SOSkill sSkill)
    {
        if(CheckMousePosition())
        {
            StopCoroutine(OnDodgeEvent(sSkill.effectValue, mousePoint));
            StartCoroutine(OnDodgeEvent(sSkill.effectValue, mousePoint));
        }
    }

    IEnumerator OnSlashEvent(SOSkill sk)
    {
        _sType = SkillType.Slash;
        State = PlayerState.Skill;
        yield return new WaitForSeconds(0.2f);
        GameObject go = PoolingManager.Pool.InstatiateAPS(sk.skillName,transform.position, Quaternion.identity, Vector3.one, gameObject);
        if(go.TryGetComponent<SlashEffect>(out SlashEffect slash) == false)
            slash = go.AddComponent<SlashEffect>();
        slash.StartEffect(mousePoint, sk.effectValue);
        yield return new WaitForSeconds(0.5f);
        mousePoint = transform.position;
        _destPos = transform.position;
        _sType = SkillType.Unknown;
        State = PlayerState.Move;
    }


    IEnumerator OnDodgeEvent(float power,Vector3 dir)
    {
        _sType = SkillType.Dodge;
        State = PlayerState.Skill;
        ActiveDodge = true;
        _rb.AddForce(dir * power, ForceMode.Impulse);
        yield return new WaitForSeconds(0.7f);

        mousePoint = transform.position;
        _destPos = transform.position;
        ActiveDodge = false;
        _sType = SkillType.Unknown;
        State = PlayerState.Move;
    }

    IEnumerator OnHealEvent(string name)
    {
        GameObject go = PoolingManager.Pool.InstatiateAPS(name, transform.position, Quaternion.identity, Vector3.one, gameObject);
        ParticleSystem particle = go.GetComponentInChildren<ParticleSystem>();
        particle.Play(true);

        while (particle.IsAlive(true))
        {
            yield return null;
        }
        go.DestroyAPS();
    }

    
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Util.ConvertEnum(TagType.Interactive)))
        {
            _nearObj = other.gameObject;
            if (_nearObj.TryGetComponent<InteractObject>(out InteractObject io))
            {
                _nearType = io.Type;
            }
            else
            {
                ClearNearObject();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Util.ConvertEnum(TagType.Interactive)))
        {
            //switch(_nearType)
            //{

            //}

            ClearNearObject();
        }
    }
}
