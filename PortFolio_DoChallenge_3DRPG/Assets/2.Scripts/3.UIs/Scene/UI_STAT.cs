using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_STAT : UI_Base
{
    enum Texts
    {
        Level,
        CurrExp,
        MaxExp,
        CurrHp,
        MaxHp,
        PlusHp,
        CurrMp,
        MaxMp,
        PlusMp,
        Damage,
        PlusDamge,
        Defense,
        PlusDefense,
    }

    PlayerStat pStat;
    List<Text> list_Texts;
    void Start()
    {
        //Init();
        InventoryManager._inst.OnChangeStat -= OnSetUI;
        InventoryManager._inst.OnChangeStat += OnSetUI;
    }

    public override void Init()
    {
        list_Texts = new List<Text>();
        Bind<Text>(typeof(Texts));
        for(int i = 0; i <= (int)Texts.PlusDefense; i++)
        {
            list_Texts.Add(Get<Text>(i));
        }
        OnSetUI();
    }

    private void Update()
    {
        if (UI_Inventory.ActivatedInventory)
            OnSetUI();
    }

    void OnSetUI()
    {
        pStat = PlayerCtrl._inst._stat;
        float exp = pStat.EXP();
        float totalExp = pStat.TotalEXP();

        list_Texts[(int)Texts.Level].text = string.Format("{0:#,###}", pStat.Level);
        if (exp > 0)
            Get<Text>((int)Texts.CurrExp).text = string.Format("{0:#,###}", exp);
        else if (exp == 0)
            Get<Text>((int)Texts.CurrExp).text = "0";
        else
            list_Texts[(int)Texts.MaxExp].text = "Error";

        if (totalExp > 0)
            list_Texts[(int)Texts.MaxExp].text = string.Format("{0:#,###}", totalExp);
        else if (totalExp == 0)
            list_Texts[(int)Texts.MaxExp].text = "0";
        else
            list_Texts[(int)Texts.MaxExp].text = "Error";
        if (pStat.HP > 0)
            list_Texts[(int)Texts.CurrHp].text = string.Format("{0:#,###}", Mathf.Min(pStat.HP, pStat.MaxHP));
        else
            list_Texts[(int)Texts.CurrHp].text = string.Format("0");
        list_Texts[(int)Texts.MaxHp].text = string.Format("{0:#,###}", pStat.MaxHP);

        if (pStat.MP > 0)
            list_Texts[(int)Texts.CurrMp].text = string.Format("{0:#,###}", Mathf.Min(pStat.MP, pStat.MaxMP));
        else
            list_Texts[(int)Texts.CurrMp].text = string.Format("0");
        list_Texts[(int)Texts.MaxMp].text = string.Format("{0:#,###}", pStat.MaxMP);

        list_Texts[(int)Texts.Damage].text = string.Format("{0:#,###}", pStat.Damage);
        list_Texts[(int)Texts.Defense].text = string.Format("{0:#,###}", pStat.Defense);
        int value = 0;
        if (pStat.PlusHP > 0)
        {
            value = Mathf.RoundToInt(pStat.PlusHP);
            list_Texts[(int)Texts.PlusHp].text = string.Format("+{0:#,###}", value);
            list_Texts[(int)Texts.PlusHp].gameObject.SetActive(true);
        }
        else
        {
            list_Texts[(int)Texts.PlusHp].text = "0";
            list_Texts[(int)Texts.PlusHp].gameObject.SetActive(false);
        }

        if (pStat.PlusMP > 0)
        {
            value = Mathf.RoundToInt(pStat.PlusMP);
            list_Texts[(int)Texts.PlusMp].text = string.Format("+{0:#,###}", value);
            list_Texts[(int)Texts.PlusMp].gameObject.SetActive(true);
        }
        else
        {
            list_Texts[(int)Texts.PlusMp].text = "0";
            list_Texts[(int)Texts.PlusMp].gameObject.SetActive(false);
        }
        if (pStat.PlusDamage > 0)
        {
            value = Mathf.RoundToInt(pStat.PlusDamage);
            list_Texts[(int)Texts.PlusDamge].text = string.Format("+{0:#,###}", value);
            list_Texts[(int)Texts.PlusDamge].gameObject.SetActive(true);
        }
        else
        {
            list_Texts[(int)Texts.PlusDamge].text = "0";
            list_Texts[(int)Texts.PlusDamge].gameObject.SetActive(false);
        }
        if (pStat.PlusDefense > 0)
        {
            value = Mathf.RoundToInt(pStat.PlusDefense);
            list_Texts[(int)Texts.PlusDefense].text = string.Format("+{0:#,###}", value);
            list_Texts[(int)Texts.PlusDefense].gameObject.SetActive(true);
        }
        else
        {
            list_Texts[(int)Texts.PlusDefense].text = "0";
            list_Texts[(int)Texts.PlusDefense].gameObject.SetActive(false);
        }
    }   
}
