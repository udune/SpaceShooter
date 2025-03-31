using System;
using System.Collections;
using UnityEngine;
using Unity.Behavior;

public class SpiderController : MonoBehaviour
{
    [SerializeField] private BehaviorGraphAgent spiderAgent;
    private float hp = 100.0f;
    
    private void Start()
    {
        spiderAgent = GetComponent<BehaviorGraphAgent>();
    }

    IEnumerator OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            spiderAgent.BlackboardReference.SetVariableValue<bool>("IsStun", true);
            hp -= 10.0f;
            if (hp <= 0)
            {
                spiderAgent.BlackboardReference.SetVariableValue<bool>("IsDead", true);
                yield break;
            }

            yield return new WaitForSeconds(0.3f);
            spiderAgent.BlackboardReference.SetVariableValue<bool>("IsStun", false);
        }

        yield break;
    }
}
