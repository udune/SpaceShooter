using UnityEngine;
using UnityEngine.Pool;

public class PoolManager<T> where T : MonoBehaviour
{
    private readonly IObjectPool<T> pool;
    
    // 생성자 선언
    public PoolManager(T prefab, int defaultCapacity = 10, int maxSize = 20)
    {
        // Pool 객체를 생성
        pool = new ObjectPool<T>
        (
            createFunc: () => Object.Instantiate(prefab),
            actionOnGet: obj => obj.gameObject.SetActive(true),
            actionOnRelease: obj => obj.gameObject.SetActive(false),
            actionOnDestroy: obj => Object.Destroy(obj.gameObject),
            collectionCheck: true,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
    }
    
    // 외부에서 접근할 메소드를 선언
    // 풀에서 가져올때 호출하는 메소드
    public T Get() => pool.Get();
    
    // Pool 객체에 사용한 객체를 반환
    public void Release(T obj) => pool.Release(obj);
}
