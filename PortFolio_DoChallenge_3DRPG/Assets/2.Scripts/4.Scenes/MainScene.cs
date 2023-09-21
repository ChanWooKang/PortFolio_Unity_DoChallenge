using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

public class MainScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        CurrScene = SceneType.MainScene;
    }
   
    public void SceneLoadToInGame()
    {
        Managers._scene.LoadScene(SceneType.InGameScene);
    }

    public override void Clear() { base.Clear(); }
}
