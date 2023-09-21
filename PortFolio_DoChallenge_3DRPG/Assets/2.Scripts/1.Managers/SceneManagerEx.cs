using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Defines;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }
    public Dictionary<CursorType, Texture2D> cursors;

    public void Init()
    {
        cursors = new Dictionary<CursorType, Texture2D>();
    }

    public void LoadScene(SceneType scene)
    {
        Managers.Clear();
        SceneManager.LoadScene(Util.ConvertEnum(scene));
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }

    public Texture2D LoadCursor(CursorType type)
    {
        if (cursors.TryGetValue(type, out Texture2D tex) == false)
        {
            switch (type)
            {
                case CursorType.Hand:
                    tex = Managers._file.LoadImageFile("Cursors/Hand");
                    break;
                case CursorType.Attack:
                    tex = Managers._file.LoadImageFile("Cursors/Attack");
                    break;
            }

            if (tex == null)
            {
                Debug.Log($"Scene Manager - Failed To Load : {type.ToString()}");
                return null;
            }
            else
            {
                cursors.Add(type, tex);
                return tex;
            }
        }
        else
        {
            return tex;
        }
    }
}
