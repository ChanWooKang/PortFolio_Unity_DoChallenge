using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

public class MonsterStateTrace : TSingleton<MonsterStateTrace>, IFSMState<MonsterCtrl>
{
    public void Enter(MonsterCtrl m)
    {
        m.Agent.SetDestination(transform.position);
    }

    public void Execute(MonsterCtrl m)
    {
        if(m.target != null)
        {
            if(m.IsCloseTarget(m.target.position,m._stat.TraceRange))
            {
                if (m.IsCloseTarget(m.target.position, m._stat.AttackRange))
                    m.ChangeState(MonsterStateAttack._inst);
                else
                {
                    if (m.State != MonsterState.Trace)
                        m.State = MonsterState.Trace;
                    m.MoveFunc(m.target.position);
                }    
            }
            else
                m.ChangeState(MonsterStatePatrol._inst);
        }
        else
            m.ChangeState(MonsterStatePatrol._inst);
    }

    public void Exit(MonsterCtrl m)
    {
        
    }
}
