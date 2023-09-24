using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Defines;


public class UI_Equipment : UI_Base
{
    enum GameObjects
    {
        Slots_Parent
    }

    Dictionary<EquipmentType, UI_EquipSlot> Equip_Slots = new Dictionary<EquipmentType, UI_EquipSlot>();
    [SerializeField] GameObject Parent_Slots;
    public Dictionary<EquipmentType, SOItem> GetData()
    {
        Dictionary<EquipmentType, SOItem> dict = new Dictionary<EquipmentType, SOItem>();

        for(int i = 0; i < (int)EquipmentType.Max_Cnt; i++)
        {
            if(Equip_Slots[(EquipmentType)i].item != null)
            {
                dict.Add((EquipmentType)i, Equip_Slots[(EquipmentType)i].item);
            }
            else
            {
                dict.Add((EquipmentType)i, null);
            }
        }

        return dict;
    }
    void Start()
    {
        //Init();
    }

    public override void Init()
    {
       
        SettingSlots();   
    }

    public void SettingSlots()
    {
        UI_EquipSlot[] slots = Parent_Slots.GetComponentsInChildren<UI_EquipSlot>();
        foreach(UI_EquipSlot slot in slots)
        {
            if (Equip_Slots.ContainsKey(slot.slotType))
            {
                Debug.Log($"존재하는 장비 슬롯 선택을 다시하세요 {slot.gameObject.name}");
                return;
            }
            Equip_Slots.Add(slot.slotType, slot);
        }
    }

    public void AcquireItem(EquipmentType type,SOItem _item)
    {
        Equip_Slots[type].SetItem(_item);
    }

    public void LoadToEquip(int arrayNum, string iName)
    {

        for (int i = 0; i < InventoryManager._inst.items.Length; i++)
        {
            if (InventoryManager._inst.items[i].Name == iName)
            {
                Equip_Slots[(EquipmentType)arrayNum].SetItem(InventoryManager._inst.items[i]);
            }
        }
    }
}
