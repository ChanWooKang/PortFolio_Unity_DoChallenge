using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Object/SkillData")]
public class SOSkill : ScriptableObject
{
    public SkillType _type;
    public string skillName;
    public KeyCode key;
    public float cool;
    public float effectValue;
    public List<STAT> sList = new List<STAT>();

}
