using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour, IDamagable
{
    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE
    }
    
    [SerializeField] private State state = State.IDLE;
    // 추적 사정거리
    [SerializeField] private float traceDist = 10.0f;
    // 공격 사정거리
    [SerializeField] private float attackDist = 2.0f;

    private Transform playerTrn;
    private Transform monsterTrn;
    private NavMeshAgent agent;
    private Animator animator;

    public bool isDie = false;
    
    private readonly int hashIsTrace = Animator.StringToHash("IsTrace");
    private readonly int hashIsAttack = Animator.StringToHash("IsAttack");
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    private readonly int hashDie = Animator.StringToHash("Die");
    
    // 몬스터 HP
    private float hp = 100.0f;
    
    // 몬스터 Collider
    private List<Collider> colliders = new List<Collider>();

    private void Awake()
    {
        // player 게임오브젝트 검색
        GameObject playerObj = GameObject.Find("Player");
        playerTrn = playerObj.GetComponent<Transform>();
        monsterTrn = transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        animator = GetComponent<Animator>();
        
        monsterTrn.GetComponentsInChildren<Collider>(colliders);
    }
    
    private void OnEnable()
    {
        PlayerController.OnPlayerDie += OnPlayerDie;
        
        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerDie -= OnPlayerDie;
    }

    void Update()
    {
        if (agent.remainingDistance >= 2.0f)
        {
            // 에이전트 이동방향
            Vector3 direction = agent.desiredVelocity;
            if (direction == Vector3.zero) 
                return;
            // 회전각
            Quaternion rotation = Quaternion.LookRotation(direction);
            // Slerp 부드러운 회전 처리
            monsterTrn.rotation = Quaternion.Slerp(monsterTrn.rotation, rotation, Time.deltaTime * 10.0f);
        }
    }

    private IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            if (state == State.DIE)
                yield break;
            
            // player와 monster간의 거리 계산
            float distance = Vector3.Distance(playerTrn.position, monsterTrn.position);

            if (distance <= attackDist)
            {
                state = State.ATTACK;
            }
            else if (distance <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.IDLE;
            }
            
            yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (state)
            {
                case State.IDLE:
                    // 추적 정지
                    agent.isStopped = true;
                    animator.SetBool(hashIsTrace, false);
                    break;
                case State.TRACE:
                    // 추적 시작
                    agent.SetDestination(playerTrn.position);
                    agent.isStopped = false;
                    // 공격 애니메이션 취소
                    animator.SetBool(hashIsAttack, false);
                    // 추적 애니메이션
                    animator.SetBool(hashIsTrace, true);
                    break;
                case State.ATTACK:
                    // 공격 애니메이션 실행
                    animator.SetBool(hashIsAttack, true);
                    break;
                case State.DIE:
                    isDie = true;
                    agent.isStopped = true;
                    animator.SetTrigger(hashDie);
                    ToggleColliders(false);
                    
                    // 일정 시간이 흐른 후 오브젝트 풀로 반환
                    yield return new WaitForSeconds(3.0f);
                    
                    // 몬스터의 각종 변수를 초기화
                    hp = 100.0f;
                    isDie = false;
                    state = State.IDLE;
                    ToggleColliders(true);
                    
                    // 오브젝트 풀로 반환
                    MonsterPool.Instance.pool.Release(this);
                    break;
            }
            
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            collision.gameObject.GetComponent<Bullet>().Release();
        }
    }

    public void OnPlayerDie()
    {
        animator.SetTrigger(hashPlayerDie);
        agent.isStopped = true;
        
        StopAllCoroutines();
    }

    void ToggleColliders(bool active)
    {
        foreach (var collider in colliders)
        {
            collider.enabled = active;
        }
    }

    public void Damage(int damage)
    {
        // 데미지 리액션
        animator.SetTrigger(hashHit);
        
        // hp 차감
        hp -= damage;
        if (hp <= 0)
        {
            state = State.DIE;
        }
    }
}
