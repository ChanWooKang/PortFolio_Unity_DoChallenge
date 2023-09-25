using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Defines;

public class UI_GOLD : UI_Base
{

    enum Texts
    {
        Text_Gold
    }

    [SerializeField] Text GoldText;

    void Start()
    {
        InventoryManager._inst.OnChangeStat -= OnSetUI;
        InventoryManager._inst.OnChangeStat += OnSetUI;
    }

    public override void Init()
    {
        
        OnSetUI();
    }

    void OnSetUI()
    {
        float gold = PlayerCtrl._inst._stat.Gold;
        if (gold > 0)
            GoldText.text = string.Format("{0:#,###}", gold);
        else
            GoldText.text = "0";
    }
}
