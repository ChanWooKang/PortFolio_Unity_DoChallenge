using Defines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Inventory : UI_Base
{
    enum GameObjects
    {
        Inventory_Base,
        Inventory_Slot_Parent,
        Close
    }

    public static bool ActivatedInventory = false;

    GameObject Inventory_Base;
    GameObject Slots_Parent;

    UI_Slot[] slots;

    public UI_Slot[] GetInvenSlots() 
    {
        return slots; 
    }

    public void LoadToInven(int arrayNum, string iName, int iCount)
    {
        for(int i = 0; i < InventoryManager._inst.items.Length; i++)
        {
            if(InventoryManager._inst.items[i].Name == iName)
            {
                slots[arrayNum].AddItem(InventoryManager._inst.items[i], iCount);
            }
        }
    }
        

    void Start()
    {
        Init();
    }

    void Update()
    {
        TryOpenInventory();
    }

    public override void Init()
    {
        BindSetting();
    }

    void BindSetting()
    {
        Bind<GameObject>(typeof(GameObjects));
        Inventory_Base = GetObject((int)GameObjects.Inventory_Base);
        Slots_Parent = GetObject((int)GameObjects.Inventory_Slot_Parent);
        slots = Slots_Parent.GetComponentsInChildren<UI_Slot>();
        GameObject go = GetObject((int)GameObjects.Close);
        BindEvent(go, (PointerEventData data) => { if (data.button == PointerEventData.InputButton.Left) CloseInventory(); }, UIEvent.Click);
    }

    void TryOpenInventory()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if (ActivatedInventory == false)
                OpenInventory();
            else
                CloseInventory();
        }
    }

    void OpenInventory()
    {
        ActivatedInventory = true;
        InventoryManager._inst.OnChangeStat?.Invoke();
        Inventory_Base.SetActive(true);
    }

    public void CloseInventory()
    {
        ActivatedInventory = false;
        Inventory_Base.SetActive(false);
    }

    public void AcquireItem(SOItem _item, int _count = 1)
    {
        if (_item.iType == ItemType.Gold)
            return;

        if(ItemType.Equipment != _item.iType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.Name == _item.Name)
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }

        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null) 
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }

    }

    public bool CheckSlotFull(SOItem _item, int _count = 1)
    {
        if (ItemType.Equipment != _item.iType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.Name == _item.Name)
                    {
                        //나중에는 맥스 스택까지 차있는지 확인하고 다음껄로 넘어가서 들어가지는지 확인만함
                        return false;
                    }
                }
            }
        }
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return false;
            }
        }

        return true;
    }
}
