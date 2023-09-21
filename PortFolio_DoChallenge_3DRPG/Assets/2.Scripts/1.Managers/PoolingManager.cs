using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

[AddComponentMenu("Custom/PoolingManager")]
public class PoolingManager : MonoBehaviour
{
    [Serializable]
    public class PoolUnit
    {
        public string name;
        public PoolType pType;
        public GameObject prefab;
        public int amount;
        int curAmount;

        public int CurAmount { get { return curAmount; } set { curAmount = value; } }
    }
    static PoolingManager uniqueInstance;
    public static PoolingManager Pool { get { Init(); return uniqueInstance; } }
    public PoolUnit[] _poolingUnits;
    public List<GameObject>[] _pooledUnitList;
    public int _defPoolAmount = 5;
    public bool _canPoolExpend = true;

    void Awake()
    {

    }

    static void Init()
    {
        if (uniqueInstance == null)
        {
            GameObject go = GameObject.Find("@Pool");
            if (go == null)
            {
                Debug.Log($"Failed to Load : PoolManager");
                return;
            }
            uniqueInstance = go.GetComponent<PoolingManager>();
        }

    }

    public void SettingData()
    {
        _pooledUnitList = new List<GameObject>[_poolingUnits.Length];

        for (int i = 0; i < _poolingUnits.Length; i++)
        {
            _pooledUnitList[i] = new List<GameObject>();
            if (_poolingUnits[i].amount > 0)
                _poolingUnits[i].CurAmount = _poolingUnits[i].amount;
            else
                _poolingUnits[i].CurAmount = _defPoolAmount;

            int index = 0;
            for (int j = 0; j < _poolingUnits[i].CurAmount; j++)
            {
                GameObject newItem = (GameObject)Instantiate(_poolingUnits[i].prefab);
                string suffix = "_" + index;
                AddToPooledUnitList(i, newItem, suffix);
                ++index;
            }
        }
    }

    public void Clear()
    {
        uniqueInstance = null;
        foreach (List<GameObject> list in _pooledUnitList)
        {
            list.Clear();
        }

    }

    void AddToPooledUnitList(int index, GameObject newItem, string suffix, Transform parent = null)
    {
        newItem.name += suffix;
        newItem.SetActive(false);
        
        if (parent == null)
            newItem.transform.SetParent(transform);
        else
            newItem.transform.SetParent(parent);
        _pooledUnitList[index].Add(newItem);
    }

    GameObject GetPooledItem(string value)
    {
        for (int unitIdx = 0; unitIdx < _pooledUnitList.Length; unitIdx++)
        {
            if (_poolingUnits[unitIdx].prefab.name == value)
            {
                int listIdx = 0;
                for (; listIdx < _pooledUnitList[unitIdx].Count; listIdx++)
                {
                    if (_pooledUnitList[unitIdx][listIdx] == null)
                        return null;
                    if (_pooledUnitList[unitIdx][listIdx].activeInHierarchy == false)
                        return _pooledUnitList[unitIdx][listIdx];
                }

                if (_canPoolExpend)
                {
                    GameObject tmpObj = (GameObject)Instantiate(_poolingUnits[unitIdx].prefab);
                    string suffix = "_" + listIdx.ToString() + "(" + (listIdx - _poolingUnits[unitIdx].CurAmount + 1).ToString() + ")";
                    AddToPooledUnitList(unitIdx, tmpObj, suffix);
                    return tmpObj;
                }
                break;
            }
        }
        return null;
    }
    public GameObject InstatiateAPS(int idx, GameObject parent = null)
    {
        string pooledUnitName = _poolingUnits[idx].name;
        Transform prefabTF = _poolingUnits[idx].prefab.transform;
        GameObject go = InstatiateAPS(pooledUnitName, Vector3.zero, prefabTF.rotation, Vector3.one, parent);

        return go;
    }

    public GameObject InstatiateAPS(
        int idx, Vector3 pos, Quaternion rot, Vector3 scale, GameObject parent = null
        )
    {
        string pooledUnitName = _poolingUnits[idx].name;
        GameObject go = InstatiateAPS(pooledUnitName, pos, rot, scale, parent);
        return null;
    }

    public GameObject InstatiateAPS(string pooledUnitName, GameObject parent = null)
    {
        GameObject go = GetPooledItem(pooledUnitName);

        if (parent != null)
            go.transform.parent = parent.transform;

        go.SetActive(true);
        return go;
    }

    public GameObject InstatiateAPS(
        string pooledUnitName, Vector3 pos, Quaternion rot, Vector3 scale, GameObject parent = null
        )
    {
        GameObject go = GetPooledItem(pooledUnitName);
        if (go != null)
        {
            if (parent != null)
                go.transform.parent = parent.transform;

            go.transform.position = pos;
            go.transform.rotation = rot;
            go.transform.localScale = scale;
            go.SetActive(true);
        }

        return go;
    }

    public List<GameObject> GetActivatePooledItem()
    {
        List<GameObject> tmps = new List<GameObject>();
        for (int unitIdx = 0; unitIdx < _poolingUnits.Length; ++unitIdx)
        {
            for (int listIdx = 0; listIdx < _pooledUnitList[unitIdx].Count; ++listIdx)
            {
                if (_pooledUnitList[unitIdx][listIdx].activeInHierarchy)
                    tmps.Add(_pooledUnitList[unitIdx][listIdx]);
            }
        }

        return tmps;
    }

    public static void DestroyAPS(GameObject go) { go.SetActive(false); }
}
