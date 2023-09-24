using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

public class SpawnManager : MonoBehaviour
{
    static SpawnManager instance;
    public static SpawnManager _inst {  get { return instance; } }
    public Action<MonsterType, int> OnSpawnEvent;
    public List<SpawnPoint> points = new List<SpawnPoint>();
    PoolingManager pool;
    float power = 5;

    void Start()
    {
        Init();
    }

    void Init()
    {
        instance = this;
        pool = PoolingManager.Pool;
    }

    MonsterType GetMonsterType(GameObject go)
    {
        MonsterType type = MonsterType.Unknown;
        if (go.TryGetComponent<MonsterCtrl>(out MonsterCtrl mc))
        {
            type = mc.mType;
        }

        return type;
    }

    public GameObject Spawn(MonsterType type)
    {
        Transform tr = null;
        for(int i = 0; i < points.Count; i++)
        {
            if(type == points[i].targetType)
            {
                tr = points[i].target;
                break;
            }
        }

        if(tr == null)
        {
            Debug.Log($"해당 스폰 위치 존재 X : {type}");
            return null;
        }

        GameObject go = pool.InstatiateAPS(Util.ConvertEnum(type));
        if(go.TryGetComponent<MonsterCtrl>(out MonsterCtrl mc) == false)
        {
            Destroy(go);
            return null;
        }

        mc._defPos = tr.position;
        OnSpawnEvent?.Invoke(type, 1);
        return go;
    }

    public void Spawn(SOItem item, Transform parent)
    {
        GameObject go = SpawnObject(item, parent);
        if(go != null)
        {
            if (go.TryGetComponent<Item>(out Item im) == false)
            {
                im = go.AddComponent<Item>();
                im.itemSO = item;
            }
            im.Shoot(power);
        }
        else
        {
            Debug.Log($"Failed to Load Prefab : {item.Name}");
            return;
        }
    }
    public void Spawn(SOItem item, Transform parent , int gold)
    {
        GameObject go = SpawnObject(item, parent);
        if (go != null)
        {
            if (go.TryGetComponent<Item>(out Item im) == false)
            {
                im = go.AddComponent<Item>();
                im.itemSO = item;
            }
            im.Shoot(power);
            im.Gold = gold;
        }
        else
        {
            Debug.Log($"Failed to Load Prefab : {item.Name}");
            return;
        }
    }

    GameObject SpawnObject(SOItem item, Transform parent)
    {
        Vector3 pos = parent.position;
        pos.y = 0;
        Quaternion rot = Quaternion.Euler(0, 0, 0);
        GameObject go = pool.InstatiateAPS(item.Name, pos, rot, Vector3.one);
        return go;
    }

    public void MonsterDespawn(GameObject go)
    {
        MonsterType type = GetMonsterType(go);
        if (type == MonsterType.Unknown || type == MonsterType.Max_Cnt)
            return;
        OnSpawnEvent?.Invoke(type, -1);
        go.DestroyAPS();
    }
}

[Serializable] 
public struct SpawnPoint
{
    public Transform target;
    public MonsterType targetType;
}