using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

public class DataManager
{
    public Dictionary<int, StatByLevel> StatDict { get; private set; } = new Dictionary<int, StatByLevel>();

    public void Init()
    {
        StatDict = LoadJson<StatData, int, StatByLevel>("StatByLevel").Make();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        string text = Managers._file.LoadJsonFile(path);
        return JsonUtility.FromJson<Loader>(text);
    }
}
