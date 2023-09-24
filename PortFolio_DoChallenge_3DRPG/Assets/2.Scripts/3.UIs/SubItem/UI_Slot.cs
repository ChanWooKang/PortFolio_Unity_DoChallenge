using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Defines;

public class UI_Slot : UI_Base , IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public SOItem item;
    public int itemCount;
    [SerializeField] Image Image_Item;
    [SerializeField] Text Text_Count;
    [SerializeField] GameObject Parent_Count;
    enum GameObjects
    {
        Item_Image,
        Count_Text,
        Count_Parent
    }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        //SettingBind();
        SetSlotCount(0);
    }

    void SettingBind()
    {
        Bind<GameObject>(typeof(GameObjects));
        Image_Item = GetObject((int)GameObjects.Item_Image).GetComponent<Image>();
        Text_Count = GetObject((int)GameObjects.Count_Text).GetComponent<Text>();
        Parent_Count = GetObject((int)GameObjects.Count_Parent);
    }

    void SetAlpah(float alpha)
    {
        Color color = Image_Item.color;
        color.a = alpha;
        Image_Item.color = color;
    }

    public void AddItem(SOItem _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        if (Image_Item == null)
            Debug.Log("11");
        Image_Item.sprite = _item.Icon;
        if (item.iType != ItemType.Equipment)
        {
            Text_Count.text = itemCount.ToString();
            Parent_Count.SetActive(true);
        }
        else
        {
            Text_Count.text = "0";
            Parent_Count.SetActive(false);
        }

        SetAlpah(1);
    }

    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        Text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
            ClearSlot();
    }

    public void ClearSlot()
    {
        item = null;
        itemCount = 0;
        Image_Item.sprite = null;
        SetAlpah(0);
        Text_Count.text = "0";
        Parent_Count.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       
        if(eventData.button == PointerEventData.InputButton.Left)
        {

        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                if(item.iType == ItemType.Equipment)
                {
                    //장비 장착
                    if (InventoryManager.IsChangingEquip == false)
                    {
                        InventoryManager._inst.OnChangeEvent?.Invoke(item.eType, item,true);
                        ClearSlot();
                    }   
                }
                else if (item.iType == ItemType.Potion)
                {
                    //포션 사용
                    InventoryManager._inst.OnUsePotionEvent(item);
                    SetSlotCount(-1);
                    //포션 이벤트
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlotCtrl.isDragFromInven = true;
            DragSlotCtrl._inst.SetCanvas(false);
            DragSlotCtrl._inst.Slot_FromInven = this;
            DragSlotCtrl._inst.DragSetImage(Image_Item);
            
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
        
        if (DragSlotCtrl.isDragFromInven)
        {
            if(DragSlotCtrl._inst.Slot_FromInven != null)
            {
                ChangeSlot();
            }
        }
        else
        {
            if (InventoryManager.IsChangingEquip == false)
            {
                if(InventoryManager._inst.CheckSlotFull(DragSlotCtrl._inst.Slot_FromEquip.item) == false)
                {
                    InventoryManager._inst.OnChangeEvent?.Invoke(DragSlotCtrl._inst.Slot_FromEquip.item.eType, DragSlotCtrl._inst.Slot_FromEquip.item, false);
                    DragSlotCtrl._inst.Slot_FromEquip.ClearSlot();
                }
            }
        }

    }

    void ChangeSlot()
    {
        SOItem tempItem = item;
        int tempCount = itemCount;
        AddItem(DragSlotCtrl._inst.Slot_FromInven.item, DragSlotCtrl._inst.Slot_FromInven.itemCount);
        if (tempItem != null)
        {
            DragSlotCtrl._inst.Slot_FromInven.AddItem(tempItem, tempCount);
        }
        else
        {
            DragSlotCtrl._inst.Slot_FromInven.ClearSlot();
        }
    }
}
