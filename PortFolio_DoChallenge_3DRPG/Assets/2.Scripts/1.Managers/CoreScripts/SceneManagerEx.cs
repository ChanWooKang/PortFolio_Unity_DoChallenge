using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Defines;
using UnityEditor;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }
    public Dictionary<CursorType, Texture2D> cursors;

    public void Init()
    {
        cursors = new Dictionary<CursorType, Texture2D>();
    }


    public IEnumerator LoadCoroutine(SceneType scene)
    {
        string sceneName = Util.ConvertEnum(scene);
        AsyncOperation oper = SceneManager.LoadSceneAsync(sceneName);
        Managers.Clear();
        while (!oper.isDone)
        {
            yield return null;
        }
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }

    public Texture2D LoadCursor(CursorType type)
    {
        if (cursors.TryGetValue(type, out Texture2D cursor) == false)
        {
            switch (type)
            {
                case CursorType.Default:
                    cursor = Managers._resource.Load<Texture2D>("Art/Cursor/Default");
                    break;
                case CursorType.Hand:
                    cursor = Managers._resource.Load<Texture2D>("Art/Cursor/Hand");
                    break;
                case CursorType.Attack:
                    cursor = Managers._resource.Load<Texture2D>("Art/Cursor/Attack");
                    break;
            }

            if (cursor == null)
            {
                Debug.Log($"Scene Manager - Failed To Load : {type}");
                return null;
            }
            else
            {
                
                cursors.Add(type, cursor);
                return cursor;
            }
        }
        else
        {
            return cursor;
        }
    }
}
