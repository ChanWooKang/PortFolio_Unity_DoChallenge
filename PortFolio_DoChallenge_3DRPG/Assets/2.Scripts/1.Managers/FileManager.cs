using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using Defines;
using UnityEngine.AI;

public class FileManager
{
    
    const string JsonSuffix = ".json";
    const string ImageSuffix = ".png";

    string JsonPath;
    string ImagePath;

    public void Init()
    {
#if UNITY_EDITOR
        JsonPath = Application.dataPath + "/DataFolder/Json/";
        ImagePath = Application.dataPath + "/DataFolder/Image/";

#else
        JsonPath = Application.persistentDataPath + "/DataFolder/Json/";
        ImagePath = Application.persistentDataPath + "/DataFolder/Image/";
#endif
        if (Directory.Exists(JsonPath) == false)
            Directory.CreateDirectory(JsonPath);
        if (Directory.Exists(ImagePath) == false)
            Directory.CreateDirectory(ImagePath);
    }

    public Texture2D LoadImageFile(string name)
    {
        try
        {
            if (name.Contains(ImageSuffix) == false)
                name += ImageSuffix;

            string path = ImagePath + name;
            if(File.Exists(path) == false)
            {
                Debug.Log($"Failed To Load Image Source : {name}");
                return null;
            }
            Byte[] bytes = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);
            return tex;
        }
        catch (Exception ex) 
        {
            Debug.Log(ex.Message);
            return null;
        }
    }

    public void CreateJsonFile(string name, string data = null)
    {
        try
        {
            if (name.Contains(JsonSuffix) == false)
                name += JsonSuffix;

            string path = JsonPath + name;
            File.WriteAllText(path, data);
        }
        catch (Exception ex) 
        {
            Debug.Log(ex.Message);
        }
    }

    public void SaveJsonFile<T>(T data, string name)
    {
        try
        {
            string saveJson = JsonUtility.ToJson(data);

            if(name.Contains(JsonSuffix) == false)
                name += JsonSuffix;

            string path = JsonPath + name;
            if(File.Exists(path) == false)
            {
                CreateJsonFile(path, saveJson);
            }
            else
            {
                File.WriteAllText(path,saveJson);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public string LoadJsonFile(string name)
    {
        try
        {
            if (name.Contains(JsonSuffix) == false)
                name += JsonSuffix;

            string path = JsonPath + name;
            if (File.Exists(path) == false)
            {
                CreateJsonFile(name,string.Empty);
                return string.Empty;
            }

            string saveJson = File.ReadAllText(path);
            return saveJson;
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            return string.Empty;
        }
    }
}
