using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class UI_HPBar : UI_Base
{
    enum Images
    {
        HP,
    }

    MonsterCtrl pmc;
    Image HP;

    public override void Init()
    {
        pmc = transform.parent.GetComponent<MonsterCtrl>();
        HP = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        
    }

    private void Start()
    {
        Init();
    }

    void Update()
    {
        Transform parent = transform.parent;
        transform.position = parent.position + Vector3.up * (pmc.Agent.height);
        transform.rotation = Camera.main.transform.rotation;

        float ratio = pmc.stat.HP / pmc.stat.MaxHP;
        SetHPBar(ratio);
    }


    public void SetHPBar(float ratio)
    {
        HP.fillAmount = ratio;
    }

}
