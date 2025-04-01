using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private static readonly int Forward = Animator.StringToHash("Forward");
    private static readonly int Strafe = Animator.StringToHash("Strafe");
    private float horizontal;
    private float vertical;
    private float rotate;

    [SerializeField]
    private float moveSpeed = 5.0f;
    
    [SerializeField]
    private float turnSpeed = 200.0f;

    // [SerializeField] 
    // private Image hpBar;
    
    private Animator animator;

    private float initHp = 100.0f;
    
    [SerializeField]
    private float currentHp = 100.0f;
    
    // 델리게이트 (delegate) : 대리자
    public delegate void PlayerDieHandler();
    
    // 이벤트 정의
    public static event PlayerDieHandler OnPlayerDie;
    
    [SerializeField]
    private ApplyDamageToPlayer applyDamageToPlayer;
    
    [SerializeField]
    private HealthEventSO healthEventSO;
    
    private InputController inputController;
    
    void OnEnable()
    {
        applyDamageToPlayer.Event += OnPlayerDamaged;
        inputController.OnMove += ctx => { horizontal = ctx.x; vertical = ctx.y; };
        inputController.OnRotate += ctx => rotate = ctx * 0.1f;
    }

    private void OnDisable()
    {
        applyDamageToPlayer.Event -= OnPlayerDamaged;
    }
    
    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        inputController = gameObject.GetComponent<InputController>();
    }

    void Update()
    {
        Move();
        Animate();
    }
    
    private void OnPlayerDamaged(Transform player, int damage)
    {
        if (currentHp > 0.0f)
        {
            currentHp -= (float)damage;
            
            healthEventSO.Raise(currentHp);
            // hpBar.fillAmount = currentHp / initHp;
            
            if (currentHp <= 0.0f)
            {
                healthEventSO.Raise(0.0f);
                // hpBar.fillAmount = 0.0f;
                GameManager.Instance.IsGameOver = true;
                OnPlayerDie?.Invoke();
            }
        }
    }

    private void Animate()
    {
        animator.SetFloat(Forward, vertical);
        animator.SetFloat(Strafe, horizontal);
    }

    void Move()
    {
        // horizontal = Input.GetAxis("Horizontal");
        // vertical = Input.GetAxis("Vertical");
        // rotate = Input.GetAxis("Mouse X");

        // transform.position += new Vector3(0, 0, 1) * 0.01f;
        // transform.position += new Vector3(1, 0, 0) * 0.01f;
        //
        // transform.position += new Vector3(0, 0, 1) * vertical * 0.01f;
        // transform.position += new Vector3(1, 0, 0) * horizontal * 0.01f;
        //
        // transform.position += transform.forward * vertical * 0.01f;
        // transform.position += transform.right * horizontal * 0.01f;
        //
        // transform.Translate(Vector3.forward * vertical * 0.01f);
        // transform.Translate(Vector3.right * horizontal * 0.01f);
        //
        // transform.Translate(Vector3.forward * vertical * Time.deltaTime * moveSpeed);
        // transform.Translate(Vector3.right * horizontal * Time.deltaTime * moveSpeed);
        //
        // Vector3 moveDir = (Vector3.forward * vertical) + (Vector3.right * horizontal);
        // transform.Translate(moveDir * Time.deltaTime * moveSpeed);
        
        Vector3 moveDir = (Vector3.forward * vertical) + (Vector3.right * horizontal);
        transform.Translate(moveDir.normalized * Time.deltaTime * moveSpeed);
        transform.Rotate(Vector3.up * Time.deltaTime * rotate * turnSpeed);
        
        //Debug.Log($"Horizontal: {horizontal}, Vertical: {vertical}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentHp > 0.0f && other.CompareTag("Punch"))
        {
            OnPlayerDamaged(null, 10);
        }
    }

    // private void PlayerDie()
    // {
    //     GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
    //     foreach (var monster in monsters)
    //     {
    //         // monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
    //         monster.GetComponent<MonsterController>().OnPlayerDie();
    //     }
    // }
}
