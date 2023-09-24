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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneLoadToInGame();
        }
    }

    public void SceneLoadToInGame()
    {
        StartCoroutine(Managers._scene.LoadCoroutine(SceneType.InGameScene));
    }

    public override void Clear() { base.Clear(); }
}
