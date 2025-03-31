using System;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance = null;
    public PoolManager<Bullet> pool;

    private void Awake()
    {
        Instance = this;
        // Pool 생성
        pool = new PoolManager<Bullet>(Resources.Load<Bullet>("Bullet"), 5, 10);
    }
}
