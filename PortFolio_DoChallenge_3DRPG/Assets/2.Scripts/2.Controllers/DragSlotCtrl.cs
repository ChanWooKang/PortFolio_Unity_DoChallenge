using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlotCtrl : UI_Base
{
    static DragSlotCtrl instance;
    public static DragSlotCtrl _inst { get { return instance; } }
    public static bool isDragFromInven = true;
    public UI_Slot Slot_FromInven;
    public UI_EquipSlot Slot_FromEquip;
    public RectTransform rect;
    CanvasGroup groupCanvas;
    
    Image ImageItem;

    void Start()
    {
        Init();
    }
    public override void Init()
    {
        instance = this;
        rect = GetComponent<RectTransform>();
        ImageItem = GetComponent<Image>();
        groupCanvas = GetComponent<CanvasGroup>();
    }

    public void DragSetImage(Image _icon)
    {
        ImageItem.sprite = _icon.sprite;
        SetAlpha(1);
    }

    public void SetAlpha(float alpha)
    {
        Color color = ImageItem.color;
        color.a = alpha;
        ImageItem.color = color;
    }

    public void SetCanvas(bool isRaycast)
    {
        if (isRaycast == false)
        {
            groupCanvas.alpha = 0.6f;
        }
        else
        {
            groupCanvas.alpha = 1;
        }
        groupCanvas.blocksRaycasts = isRaycast;
    }
}
