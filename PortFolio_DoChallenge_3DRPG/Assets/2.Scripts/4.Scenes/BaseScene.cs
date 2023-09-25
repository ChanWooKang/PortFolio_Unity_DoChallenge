using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;
using UnityEngine.EventSystems;

public class BaseScene : MonoBehaviour
{
    void Awake() { Init(); }
    public SceneType PrevScene { get; protected set; } = SceneType.UnKnown;
    public SceneType CurrScene { get; protected set; } = SceneType.UnKnown;
    protected virtual void Init() 
    {
        //PoolingManager.Pool.SettingData();
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        if (obj == null)
            Managers._resource.Instantiate("UI/EventSystem").name = "@EventSystem";
    }
    public virtual void Clear() { PrevScene = CurrScene; }
}
