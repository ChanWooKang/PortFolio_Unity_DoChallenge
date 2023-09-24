using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Defines;
using System;

public static class Util
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if(component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static string ConvertEnum<T>(T value)
    {
        return System.Enum.GetName(typeof(T), value);
    }

    public static T FindChild<T>(GameObject go , string name = null , bool recursive = false) where T : UnityEngine.Object
    {
        if(go == null)
            return null;

        if(recursive == false)
        {
            for(int i = 0; i < go.transform.childCount; i++)
            {
                Transform  child = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || child.name == name)
                {
                    if (child.TryGetComponent<T>(out T component))
                        return component;
                }
            }
        }
        else
        {
            foreach(T component in go.GetComponentsInChildren<T>())
            {
                if(string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }
        return null;
    }

    public static GameObject FindChild(GameObject go , string name = null, bool recursive = false)
    {
        Transform tr = FindChild<Transform>(go, name, recursive);
        if(tr == null)
            return null;
        return tr.gameObject;
    }
}

public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component { return Util.GetOrAddComponent<T>(go);}
    public static void DestroyAPS(this GameObject go) { PoolingManager.DestroyAPS(go); }
    public static void BindEvent(this GameObject go, Action<PointerEventData> action, UIEvent type = UIEvent.Click) { UI_Base.BindEvent(go, action, type); }
}


