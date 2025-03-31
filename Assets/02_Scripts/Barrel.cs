using System;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class Barrel : MonoBehaviour, IDamagable
{
    [SerializeField] private int hitCount;
    [SerializeField] private BarrelDataSO barrelDataSo;
    
    private new AudioSource audio;
    private CinemachineImpulseSource impulseSource;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // private void OnCollisionEnter(Collision other)
    // {
    //     if (other.collider.CompareTag("Bullet"))
    //     {
    //         if (++hitCount == 3)
    //         {
    //             ExpBarrel();
    //         }
    //     }
    // }

    public void Damage(int damage)
    {
        if (++hitCount == 3)
        {
            ExpBarrel();
        }
    }
    
    private void ExpBarrel()
    {
        // 폭발 원점
        Vector3 impactPoint = transform.position + Random.insideUnitSphere * 2.0f;
        
        var rb = gameObject.AddComponent<Rigidbody>();
        rb.AddExplosionForce(1500.0f, impactPoint, 5.0f, 1800.0f);
        
        var effect = Instantiate(barrelDataSo.explosionPrefab, transform.position, Quaternion.identity);
        audio.PlayOneShot(barrelDataSo.expAudioClip, 0.8f);
        
        Destroy(effect, 2.5f);
        Destroy(gameObject, 2.0f);

        impulseSource.GenerateImpulse(Random.Range(0.1f, 0.8f));
    }
}
