using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T instance;
    static object _lock = new object();

    public static T _inst
    {
        get
        {
            lock (_lock)
            {
                if(instance == null)
                {
                    instance = (T)FindObjectOfType(typeof(T));
                    if(FindObjectsOfType(typeof(T)).Length > 1 )
                        return instance;

                    if(instance == null)
                    {
                        GameObject go = new GameObject();
                        instance = go.AddComponent<T>();
                        go.name = typeof(T).ToString();
                        go.hideFlags = HideFlags.HideAndDontSave;
                    }
                }
            }

            return instance;
        }
    }
}
