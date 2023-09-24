using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

public class MonsterStateDie : TSingleton<MonsterStateDie>, IFSMState<MonsterCtrl>
{
    public void Enter(MonsterCtrl m)
    {
        m.ChangeLayer(LayerType.MonsterDisable);
        m.State = MonsterState.Die;
        m.cntTime = 0;
        StartCoroutine(DeadAction(m));
    }

    public void Execute(MonsterCtrl m)
    {
    }

    public void Exit(MonsterCtrl m)
    {

    }

    IEnumerator DeadAction(MonsterCtrl m)
    {
        yield return new WaitForSeconds(m.delayTime);
        m.OnDeadEvent();       
    }
}
