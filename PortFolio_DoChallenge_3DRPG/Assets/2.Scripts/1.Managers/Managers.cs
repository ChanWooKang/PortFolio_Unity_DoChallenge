using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers instance;
    static Managers _inst { get { Init(); return instance; } }
    
    DataManager Data = new DataManager();
    FileManager File = new FileManager();
    InputManager Input = new InputManager();
    ResourceManager Resource = new ResourceManager();
    SceneManagerEx Scene = new SceneManagerEx();
    SoundManager Sound = new SoundManager();
    UIManager UI = new UIManager();

    public static DataManager _data { get { return _inst.Data; } }
    public static FileManager _file { get { return _inst.File; } }
    public static InputManager _input { get { return _inst.Input; } }
    public static ResourceManager _resource { get { return _inst.Resource; } }
    public static SceneManagerEx _scene { get { return _inst.Scene; } }
    public static SoundManager _sound { get { return _inst.Sound; } }
    public static UIManager _ui { get { return _inst.UI; } }

    static void Init()
    {
        if(instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if(go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);
            instance = go.GetComponent<Managers>();

            instance.Scene.Init();
            instance.File.Init();
            instance.Data.Init();
            instance.Sound.Init();
        }
    }

    public static void Clear()
    {
        _input.Clear();
        _scene.Clear();
        _sound.Clear();
        _ui.Clear();
    }


    void Start()
    {
        Init();
    }

    void Update()
    {
        _input.OnUpdate();
    }
}
