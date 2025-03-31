using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FindAllWayPoints", story: "모든 포인트를 검색해서 [WayPoints] 에 추가", category: "Action", id: "8c6cd4fb216c13eb4b34efcbf2475743")]
public partial class FindAllWayPointsAction : Action
{
    [SerializeReference] public BlackboardVariable<List<GameObject>> WayPoints;

    protected override Status OnStart()
    {
        var points = GameObject.Find("WayPointGroup").GetComponentsInChildren<Transform>();
        foreach (var point in points)
        {
            WayPoints.Value.Add(point.gameObject);
        }
        
        WayPoints.Value.RemoveAt(0);
        
        return Status.Running;
    }
}

