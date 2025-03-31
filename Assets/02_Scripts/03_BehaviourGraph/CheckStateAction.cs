using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckState", story: "[SpiderDataSO] 를 이용해서 [Self] 와 [Player] 의 거리로 [State] 를 결정", category: "Action", id: "38d9bdf7eb6979c528d3d7d244a806cf")]
public partial class CheckStateAction : Action
{
    [SerializeReference] public BlackboardVariable<SpiderDataSO> SpiderDataSO;
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Transform> Player;
    [SerializeReference] public BlackboardVariable<State> State;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        // Vector3.Distance(a,b)
        // (a - b).sqrMagnitude
        float distance = (Self.Value.transform.position - Player.Value.position).sqrMagnitude;
        if (distance <= SpiderDataSO.Value.attackDistance * SpiderDataSO.Value.attackDistance)
        {
            State.Value = global::State.Attack;
        }
        else if (distance <= SpiderDataSO.Value.traceDistance * SpiderDataSO.Value.traceDistance)
        {
            State.Value = global::State.Trace;
        }
        else
        {
            State.Value = global::State.Patrol;
        }
        
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

