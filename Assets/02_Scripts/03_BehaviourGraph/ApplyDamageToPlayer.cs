using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/ApplyDamageToPlayer")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "ApplyDamageToPlayer", message: "[Player] 에게 [Damage] 를 전달", category: "Events", id: "f726dfa3bc1edeba363ca0688c535aa9")]
public partial class ApplyDamageToPlayer : EventChannelBase
{
    public delegate void ApplyDamageToPlayerEventHandler(Transform Player, int Damage);
    public event ApplyDamageToPlayerEventHandler Event; 

    public void SendEventMessage(Transform Player, int Damage)
    {
        Event?.Invoke(Player, Damage);
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        BlackboardVariable<Transform> PlayerBlackboardVariable = messageData[0] as BlackboardVariable<Transform>;
        var Player = PlayerBlackboardVariable != null ? PlayerBlackboardVariable.Value : default(Transform);

        BlackboardVariable<int> DamageBlackboardVariable = messageData[1] as BlackboardVariable<int>;
        var Damage = DamageBlackboardVariable != null ? DamageBlackboardVariable.Value : default(int);

        Event?.Invoke(Player, Damage);
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        ApplyDamageToPlayerEventHandler del = (Player, Damage) =>
        {
            BlackboardVariable<Transform> var0 = vars[0] as BlackboardVariable<Transform>;
            if(var0 != null)
                var0.Value = Player;

            BlackboardVariable<int> var1 = vars[1] as BlackboardVariable<int>;
            if(var1 != null)
                var1.Value = Damage;

            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as ApplyDamageToPlayerEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as ApplyDamageToPlayerEventHandler;
    }
}

