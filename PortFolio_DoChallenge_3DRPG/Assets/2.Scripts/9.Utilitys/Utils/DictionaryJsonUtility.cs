using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DataDictionary<TKey, TValue>
{
    public TKey Key;
    public TValue Value;
}

[System.Serializable]
public class JsonDataArray<TKey, TValue>
{
    public List<DataDictionary<TKey, TValue>> data;
}


public static class DictionaryJsonUtility
{
    public static string ToJson<TKey, TValue>(Dictionary<TKey, TValue> jsonDict, bool pretty = false)
    {
        List<DataDictionary<TKey,TValue>> dataList = new List<DataDictionary<TKey,TValue>>();
        DataDictionary<TKey, TValue> dictData;
        foreach(TKey key in jsonDict.Keys)
        {
            dictData = new DataDictionary<TKey, TValue>();
            dictData.Key = key;
            dictData.Value = jsonDict[key];
            dataList.Add(dictData);
        }
        JsonDataArray<TKey, TValue> arrayJson = new JsonDataArray<TKey, TValue>();
        arrayJson.data = dataList;
        return JsonUtility.ToJson(arrayJson,pretty);
    }

    public static Dictionary<TKey, TValue> FromJson<TKey, TValue>(string JsonData)
    {
        List<DataDictionary<TKey, TValue>> dataList = JsonUtility.FromJson<List<DataDictionary<TKey, TValue>>>(JsonData);
        Dictionary<TKey, TValue> dataDict = new Dictionary<TKey, TValue>();
        for(int i = 0; i < dataList.Count; i++)
        {
           DataDictionary<TKey,TValue> dict = dataList[i];
            dataDict[dict.Key] = dict.Value;
        }
        return dataDict;
    }
}
