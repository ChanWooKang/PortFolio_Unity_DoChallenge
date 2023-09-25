using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        CurrScene = Defines.SceneType.InGameScene;

        PoolingManager.Pool.SettingData();
    }

    public override void Clear()
    {
        base.Clear();
        
        if (InventoryManager._inst != null)
        {
            InventoryManager._inst.InventorySave();
            InventoryManager._inst.EquipmentSave();
        }

    }

    void OnApplicationQuit()
    {
        //Managers.Clear();
        Clear();
    }
}
