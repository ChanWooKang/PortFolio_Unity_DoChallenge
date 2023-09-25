using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Object/ItemData")]
public class SOItem : ScriptableObject
{
    public string Name;
    public ItemType iType;
    public EquipmentType eType;
    public int maxStack;
    public int price;
    [Multiline]
    public string description;
    public List<STAT> sList = new List<STAT>();
    public Sprite Icon;
  
}
