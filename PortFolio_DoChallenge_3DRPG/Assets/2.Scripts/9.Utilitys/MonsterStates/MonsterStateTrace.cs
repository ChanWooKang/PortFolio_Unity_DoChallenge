using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

public class MonsterStateTrace : TSingleton<MonsterStateTrace>, IFSMState<MonsterCtrl>
{
    public void Enter(MonsterCtrl m)
    {
        //m.Agent.SetDestination(transform.position);
        m.Agent.speed = m.stat.TraceSpeed;
    }

    public void Execute(MonsterCtrl m)
    {
        if(m.target != null)
        {
            if(m.IsCloseTarget(m.target.position,m.stat.TraceRange))
            {
                if (m.State != MonsterState.Trace)
                    m.State = MonsterState.Trace;
                m.MoveFunc(m.target.position);
                if (m.IsCloseTarget(m.target.position, m.stat.AttackRange))
                    m.ChangeState(MonsterStateAttack._inst);
                 
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
