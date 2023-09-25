using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Defines;

public class UI_EquipSlot : UI_Base , IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public EquipmentType slotType;
    public SOItem item = null;
    public Image Image_Item;

    enum GameObjects
    {
        Item_Image
    }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Image_Item = GetObject((int)GameObjects.Item_Image).GetComponent<Image>();
        ClearSlot();
    }

    void SetAlpha(float alpha)
    {
        Color color = Image_Item.color;
        color.a = alpha;
        Image_Item.color = color;
    }

    public void SetItem(SOItem _item)
    {
        if (_item != null)
        {
            item = _item;
            Image_Item.sprite = item.Icon;
            SetAlpha(1);
        }
        else
            ClearSlot();
    }

    public void ClearSlot()
    {
        item = null;
        Image_Item.sprite = null;
        SetAlpha(0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {

        }
        else if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(item != null)
            {
                if (InventoryManager.IsChangingEquip == false)
                {
                    if (InventoryManager._inst.CheckSlotFull(item) == false)
                    {
                        InventoryManager._inst.OnChangeEvent?.Invoke(slotType, item, false);
                        ClearSlot();
                    }
                    else
                    {
                        Debug.Log("해제 불가 /인벤토리가 꽉찼습니다....");
                    }
                        
                   
                }
            }
        }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            if (InventoryManager.IsChangingEquip == false)
            {
                if (InventoryManager._inst.CheckSlotFull(item) == false)
                {
                    DragSlotCtrl.isDragFromInven = false;
                    DragSlotCtrl._inst.Slot_FromEquip = this;
                    DragSlotCtrl._inst.DragSetImage(Image_Item);
                    DragSlotCtrl._inst.SetCanvas(false);
                }
            }
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlotCtrl._inst.rect.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlotCtrl._inst.SetAlpha(0);
        DragSlotCtrl._inst.SetCanvas(true);
        DragSlotCtrl._inst.Slot_FromInven = null;
        DragSlotCtrl._inst.Slot_FromEquip = null;
        DragSlotCtrl._inst.rect.position = Vector2.zero;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(DragSlotCtrl._inst.Slot_FromInven != null && DragSlotCtrl._inst.Slot_FromInven.item.iType == ItemType.Equipment)
        {
            if(DragSlotCtrl._inst.Slot_FromInven.item.eType == slotType)
            {
                if (InventoryManager.IsChangingEquip == false)
                {
                    InventoryManager._inst.OnChangeEvent?.Invoke(DragSlotCtrl._inst.Slot_FromInven.item.eType, DragSlotCtrl._inst.Slot_FromInven.item, true);
                    DragSlotCtrl._inst.Slot_FromInven.ClearSlot();
                }
            }
        }
    }
}
