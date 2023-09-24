using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

[System.Serializable]
public class ItemData
{
    public string Name;
    public ItemType iType;
    public EquipmentType eType;
    public int maxStack;
    public int price;
    [Multiline]
    public string description;

    public List<STAT> sList = new List<STAT>();

    public ItemData(string _name, ItemType type1, EquipmentType type2, int max , int price1, string des)
    {
        Name = _name;
        iType = type1;
        eType = type2;
        maxStack = max;
        price = price1;
        description = des;
    }
    public void Init(string name, ItemType iT, EquipmentType eT, int stack, int _price,string des ,List<STAT> list)
    {
        Name = name;
        iType = iT;
        eType = eT;
        maxStack = stack;
        price = _price;
        description = des;
        sList = list;
    }

    public Sprite LoadImage()
    {
        Texture2D tex = Managers._file.LoadImageFile($"Item/{Name}");
        Rect rect = new Rect(0,0,tex.width,tex.height); 
        return Sprite.Create(tex, rect, new Vector2(0.5f, 0.5f));
    }
}
