using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    // 싱글턴 인스턴스 선언
    public static GameManager Instance = null;
    
    [SerializeField] private List<Transform> points = new List<Transform>();
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private float createTime = 3.0f;

    // getter/setter
    private bool isGameOver = false;
    public bool IsGameOver
    {
        get => isGameOver;
        set
        {
            isGameOver = value;
            if (isGameOver)
            {
               CancelInvoke(nameof(CreateMonster)); 
            }
        }
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        var spawnPointGroupTrn = GameObject.Find("SpawnPointGroup")?.transform;
        if (spawnPointGroupTrn != null) 
            spawnPointGroupTrn.GetComponentsInChildren<Transform>(points);
        
        InvokeRepeating(nameof(CreateMonster), 2.0f, createTime);
    }

    void CreateMonster()
    {
        // 난수 발생
        int idx = Random.Range(1, points.Count);
        // var monster = Instantiate(monsterPrefab);
        var monster = MonsterPool.Instance.pool.Get();
        monster.transform.position = points[idx].position;
        monster.transform.rotation = Quaternion.identity;
    }
}
