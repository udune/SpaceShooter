using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GetRandomWayPoint", story: "[WayPoints] 에서 랜덤한 [NextWayPoint] 를 추출", category: "Action", id: "c0aadcf702a63b1d628334092d2080e4")]
public partial class GetRandomWayPointAction : Action
{
    [SerializeReference] public BlackboardVariable<List<GameObject>> WayPoints;
    [SerializeReference] public BlackboardVariable<Transform> NextWayPoint;
    
    protected override void OnEnd()
    {
        var randomIndex = Random.Range(0, WayPoints.Value.Count);
        NextWayPoint.Value = WayPoints.Value[randomIndex].transform;
    }
}

