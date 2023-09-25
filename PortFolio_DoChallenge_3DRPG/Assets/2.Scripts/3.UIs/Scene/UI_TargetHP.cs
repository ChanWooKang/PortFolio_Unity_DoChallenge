using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TargetHP : UI_Base
{
    enum GameObjects
    {
        TargetHP_Base,
        TargetName,
        TargetHP
    }

    GameObject TargetBase;
    Text TargetName;
    Image TargetHP;

    bool isOpenTarget = false;

    void Start()
    {
        Init();
        CloseLockTarget();
    }

    void Update()
    {
        TryLockObject();
        
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        TargetBase = GetObject((int)GameObjects.TargetHP_Base);
        TargetName = GetObject((int)GameObjects.TargetName).GetComponent<Text>();
        TargetHP = GetObject((int)GameObjects.TargetHP).GetComponent<Image>();
    }

    void TryLockObject()
    {
        if(PlayerCtrl._inst.LockTarget != null)
        {
            if (isOpenTarget == false)
                OpenLockTarget();
            else
                SettingUI();
        }
        else
        {
            if (isOpenTarget == true)
                CloseLockTarget();
        }
    }

    void OpenLockTarget()
    {
        isOpenTarget = true;
        TargetBase.SetActive(true);
    }

    void CloseLockTarget()
    {
        isOpenTarget = false;
        TargetBase.SetActive(false);
    }

    void SettingUI()
    {
        if (PlayerCtrl._inst.LockTarget != null)
        {
            GameObject go = PlayerCtrl._inst.LockTarget;
            if (go.TryGetComponent<MonsterCtrl>(out MonsterCtrl mc))
            {
                TargetName.text = Util.ConvertEnum(mc.mType);
                float ratio = mc.stat.HP / mc.stat.MaxHP;
                SetImage(ratio);
            }
            else
            {
                CloseLockTarget();
            }
        }
        else
            CloseLockTarget();
    }

    void SetImage(float ratio)
    {
        TargetHP.fillAmount = ratio;
    }
}
