using System;
using UnityEngine;

public class MonsterPool : MonoBehaviour
{
    public static MonsterPool Instance = null;
    public PoolManager<MonsterController> pool;

    private void Awake()
    {
        Instance = this;
        pool = new PoolManager<MonsterController>(Resources.Load<MonsterController>("Monster"));
    }
}
