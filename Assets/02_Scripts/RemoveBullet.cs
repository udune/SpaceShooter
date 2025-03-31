using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    [SerializeField] SparkEffectSO sparkEffectSO;
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Destroy(collision.gameObject);
            collision.gameObject.GetComponent<Bullet>().Release();
            
            ContactPoint cp = collision.GetContact(0);
            Vector3 point = cp.point;
            Vector3 normal = -cp.normal;
            Quaternion rot = Quaternion.LookRotation(normal);

            GameObject effect = Instantiate(sparkEffectSO.effectPrefab, point, rot);
            Destroy(effect, 0.6f);
        }
    }
}

/*
 * - Collider IsTrigger 언체크
 * OnCollisionEnter
 * OnCollisionStay
 * OnCollisionExit
 *
 * - Collider IsTrigger 체크
 * OnTriggerEnter
 * OnTriggerStay
 * OnTriggerExit
 *
 * - 충돌 감지를 위한 필수 조건
 * 1. 양쪽 모두 Collider 컴포넌트를 존재
 * 2. 움직이는 게임 오브젝트에 Rigidbody 컴포넌트가 존재
 */