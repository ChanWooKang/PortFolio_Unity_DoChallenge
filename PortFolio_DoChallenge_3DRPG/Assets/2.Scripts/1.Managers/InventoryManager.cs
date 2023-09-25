using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

public class InventoryManager : MonoBehaviour
{
    static InventoryManager instance;
    public static bool IsChangingEquip = false;
    public Action<EquipmentType, SOItem, bool> OnChangeEvent;
    public Action OnChangeStat;
    public Dictionary<EquipmentType, SOItem> Equip_DIct = new Dictionary<EquipmentType, SOItem>();
    public static InventoryManager _inst { get { return instance; } }

    public SOItem[] items;

    UI_Inventory inven;
    UI_Equipment equip;
    UI_STAT stat;
    UI_GOLD gold;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        OnChangeEvent -= ChangeEquipment;
        OnChangeEvent += ChangeEquipment;
        Init();
    }

    public void Init()
    {

        //딕셔너리 로드로 장비 장착 여부 확인
        EquipmentLoad();
        InventoryLoad();

        if (stat == null)
            stat = FindObjectOfType<UI_STAT>();
        stat.Init();
        if (gold == null)
            gold = FindObjectOfType<UI_GOLD>();
        gold.Init();

        if (inven == null)
            inven = FindObjectOfType<UI_Inventory>();

        inven.CloseInventory();

        
    }

    void ChangeEquipment(EquipmentType type , SOItem item , bool isWear = true)
    {
        StopCoroutine(ChangeCoroutine(type,item, isWear));
        StartCoroutine(ChangeCoroutine(type, item, isWear));
    }

    IEnumerator ChangeCoroutine(EquipmentType type, SOItem item , bool isWear = true)
    {
        if (item == null)
            yield break;

        IsChangingEquip = true;
        if (isWear == false)
        {
            if (Equip_DIct.ContainsKey(type) && Equip_DIct[type] == item)
            {
                if (Equip_DIct[type].sList != null)
                {
                    List<STAT> list_stats = Equip_DIct[type].sList;
                    for (int i = 0; i < list_stats.Count; i++)
                    {
                        float reverse = -list_stats[i].sValue;
                        PlayerCtrl._inst.AddPlusStat(list_stats[i].sType, reverse);
                    }
                }
                Equip_DIct[type] = null;
                AddInvenItem(item);
                AddEquipItem(type, null);
            }
        }
        else
        {
            SOItem tempItem = null;
            List<STAT> tempStats;

            if (item != null)
            {
                if (Equip_DIct.ContainsKey(type))
                {
                    //장착 되어 있던 스텟 제거
                    if (Equip_DIct[type] != null)
                    {
                        tempItem = Equip_DIct[type];
                        if (tempItem.sList != null)
                        {
                            tempStats = tempItem.sList;
                            for (int i = 0; i < tempStats.Count; i++)
                            {
                                float ReverseData = -tempStats[i].sValue;
                                PlayerCtrl._inst.AddPlusStat(tempStats[i].sType, ReverseData);

                            }
                        }
                    }

                    //장착 시킬 아이템 스텟 추가
                    if (item.sList != null)
                    {
                        tempStats = item.sList;
                        for (int i = 0; i < tempStats.Count; i++)
                        {
                            PlayerCtrl._inst.AddPlusStat(tempStats[i].sType, tempStats[i].sValue);
                        }
                    }

                    if (tempItem != null)
                    {
                        //인벤토리로 이동 시켜줘야함
                        AddInvenItem(tempItem);
                    }
                    //장비창 UI에도 적용 
                    AddEquipItem(type, item);
                    Equip_DIct[type] = item;
                }
 
            }
        }

        OnChangeStat?.Invoke();
        yield return new WaitForSeconds(1.0f);
        IsChangingEquip = false;
    }

    public bool CheckSlotFull(SOItem _item, int count = 1)
    {
        if (inven == null)
            inven = FindObjectOfType<UI_Inventory>();

        if (inven.CheckSlotFull(_item, count))
        {
            //꽉찬거
            return true;
        }
        else
        {
            //꽉 안찬거
            return false;
        }
    }

    public void AddInvenItem(SOItem _item , int cnt = 1)
    {
        if(inven == null)
            inven = FindObjectOfType<UI_Inventory>();

        inven.AcquireItem(_item, cnt);
    }

    public void AddEquipItem(EquipmentType type, SOItem item)
    {
        if(equip == null)
            equip = FindObjectOfType<UI_Equipment>();
        equip.AcquireItem(type, item);
    }

    public void OnUsePotionEvent(SOItem _item)
    {
        if(_item.sList != null)
        {
            for(int i = 0; i <_item.sList.Count; i++)
            {
                PlayerCtrl._inst.UsePotion(_item.sList[i].sType, _item.sList[i].sValue);
                OnChangeStat?.Invoke();
                Debug.Log($"포션 사용({i}) : {_item.sList[i].sName} + {_item.sList[i].sValue}");
            }
        }
    }

    public void InventorySave()
    {
        if (inven == null)
            inven = FindObjectOfType<UI_Inventory>();

        SaveInventoryData saveData = new SaveInventoryData();
        UI_Slot[] slots = inven.GetInvenSlots();
        for(int i = 0; i < slots.Length; i++)
        {
            if(slots[i].item != null)
            {
                saveData.ArrayNumbers.Add(i);
                saveData.ItemNames.Add(slots[i].item.Name);
                saveData.ItemCounts.Add(slots[i].itemCount);
            }
        }
        Managers._file.SaveJsonFile<SaveInventoryData>(saveData, "InventoryData");
    }

    public void EquipmentSave()
    {
        if (equip == null)
            equip = FindObjectOfType<UI_Equipment>();

        SaveEquipData saveData = new SaveEquipData();
        for(int i = 0; i <(int)EquipmentType.Max_Cnt; i++)
        {
            if (Equip_DIct.ContainsKey((EquipmentType)i))
            {
                if(Equip_DIct[(EquipmentType)i] != null)
                {
                    saveData.ArrayNum.Add(i);
                    saveData.ItemName.Add(Equip_DIct[(EquipmentType)i].Name);
                }
            }
        }
        Managers._file.SaveJsonFile<SaveEquipData>(saveData, "EquipmentData");

    }

    public void InventoryLoad()
    {
        if (inven == null)
            inven = FindObjectOfType<UI_Inventory>();

        string JsonData = Managers._file.LoadJsonFile("InventoryData");
        SaveInventoryData saveData = JsonUtility.FromJson<SaveInventoryData>(JsonData);
        for(int i = 0; i < saveData.ItemNames.Count; i++)
        {
            inven.LoadToInven(saveData.ArrayNumbers[i], saveData.ItemNames[i], saveData.ItemCounts[i]);
        }
    }

    public void EquipmentLoad()
    {
        if (equip == null)
        {
            equip = FindObjectOfType<UI_Equipment>();
            equip.SettingSlots();
        }
            
        
        string JsonData = Managers._file.LoadJsonFile("EquipmentData");

        if (string.IsNullOrEmpty(JsonData))
        {
            Equip_DIct = new Dictionary<EquipmentType, SOItem>();
            for (int i = 0; i < (int)EquipmentType.Max_Cnt; i++)
            {
                Equip_DIct.Add((EquipmentType)i, null);
            }
        }
        else
        {
            SaveEquipData saveData = JsonUtility.FromJson<SaveEquipData>(JsonData);
            for(int i = 0; i < saveData.ItemName.Count; i++)
            {
                equip.LoadToEquip(saveData.ArrayNum[i], saveData.ItemName[i]);
            }
            Equip_DIct = equip.GetData();
        }
    }
}


[System.Serializable]
public class SaveInventoryData
{
    public List<int> ArrayNumbers = new List<int>();
    public List<string> ItemNames = new List<string>();
    public List<int> ItemCounts = new List<int>();
}

[System.Serializable]
public class SaveEquipData
{
    public List<int> ArrayNum = new List<int>();
    public List<string> ItemName = new List<string>();
}