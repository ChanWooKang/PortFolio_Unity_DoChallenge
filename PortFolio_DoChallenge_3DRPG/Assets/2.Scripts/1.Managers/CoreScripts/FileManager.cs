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
    }

    public Texture2D LoadImageFile(string name)
    {
        try
        {

            if (Directory.Exists(ImagePath) == false)
                Directory.CreateDirectory(ImagePath);

            if (name.Contains(ImageSuffix) == false)
                name += ImageSuffix;

            string path = ImagePath + name;
            if(File.Exists(path) == false)
            {
                Debug.Log($"Failed To Load Image Source : {name}");
                return null;
            }
            Byte[] bytes = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2,2, TextureFormat.RGBA32, false);
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
            if (Directory.Exists(JsonPath) == false)
                Directory.CreateDirectory(JsonPath);

            if (name.Contains(JsonSuffix) == false)
                name += JsonSuffix;
            if(name.Contains(JsonPath) == false)
                name = JsonPath + name;
            File.WriteAllText(name, data);
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
                CreateJsonFile(name, saveJson);
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

    public void SaveJsonFile(string data , string name)
    {
        try
        {
            if (Directory.Exists(JsonPath) == false)
                Directory.CreateDirectory(JsonPath);
            if (name.Contains(JsonSuffix) == false)
                name += JsonSuffix;
            string path = JsonPath + name;
            File.WriteAllText(path, data);

        }
        catch(Exception ex)
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
