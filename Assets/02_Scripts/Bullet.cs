using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BulletDataSO bulletDataSo;

    private TrailRenderer trail;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
    }

    public void Shoot()
    {
        // rigidbody 회전값을 초기화
        rb.rotation = Quaternion.LookRotation(transform.forward);
        rb.AddRelativeForce(Vector3.forward * bulletDataSo.speed);
        Invoke(nameof(Release), 5.0f);
    }

    public void Release()
    {
        if (!gameObject.activeSelf)
            return;
        rb.rotation = Quaternion.identity;
        rb.linearVelocity = rb.angularVelocity = Vector3.zero;
        trail.Clear();
        
        BulletPool.Instance.pool.Release(this);
    }
}
