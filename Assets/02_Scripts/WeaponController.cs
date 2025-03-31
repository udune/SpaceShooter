using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePos;
    [SerializeField] private AudioClip fireSfx;

    [SerializeField] private MeshRenderer muzzleFlash;
    [SerializeField] private Animator crossHairAnim;
    [SerializeField] private SpriteRenderer aimDot;
    
    // [SerializeField] 
    // private Image magazine;
    [SerializeField]
    private AudioClip reloadSfx;
    
    [SerializeField]
    private MagazineEventSO magazineEventSO;

    private new AudioSource audio;
    private CinemachineImpulseSource impulseSource;

    // 연사로직
    private float fireRate = 0.1f;
    private float nextFire = 0.0f;
    
    // 재장전 로직
    private int maxBullet = 10;
    private int currentBullet = 10;
    private float reloadTime = 0.5f;
    private bool isReloading = false;
    
    private readonly int hashFire = Animator.StringToHash("Fire");
    private bool isFire => Input.GetMouseButton(0);
    
    // 카메라 FOV 초깃값 저장
    private float originFOV;
    
    // DeadZone Width 초깃값 저장
    private Vector2 originSize;
    
    // Cinemachine Camera
    private CinemachineCamera cinemachineCamera;
    private CinemachinePositionComposer positionComposer;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        crossHairAnim = transform.Find("CrossHair").GetComponent<Animator>();
        
        var cinemachineObj = GameObject.Find("CinemachineCamera");
        cinemachineCamera = cinemachineObj.GetComponent<CinemachineCamera>();
        positionComposer = cinemachineObj.GetComponent<CinemachinePositionComposer>();
        
        // 카메라 FOV 초깃값 저장
        originFOV = cinemachineCamera.Lens.FieldOfView;
        originSize = positionComposer.Composition.DeadZone.Size;
        
        muzzleFlash = firePos.GetComponentInChildren<MeshRenderer>();
        muzzleFlash.enabled = false;
    }

    private void Update()
    {
        // 마우스 왼쪽 버튼 클릭여부 확인
        if (!isReloading && isFire && Time.time > nextFire)
        {
            // 다음 발사할 시간을 설정
            nextFire = Time.time + fireRate;
            
            Fire();
            FireSfx();
            
            StartCoroutine(ShowMuzzleFlash());

            currentBullet--;
            
            // magazine.fillAmount = (float) currentBullet / (float) maxBullet;
            magazineEventSO.Raise(currentBullet, maxBullet);

            if (currentBullet <= 0)
            {
                isReloading = true;
                StartCoroutine(Reload());
            }
        }
        
        // 줌 기능
        ChangeFOV();
        // Aim Dot 변경 (몬스터를 조준)
        // DetectMonster();
    }

    IEnumerator Reload()
    {
        audio.PlayOneShot(reloadSfx);
        yield return new WaitForSeconds(reloadTime);
        currentBullet = maxBullet;
        isReloading = false;
        magazineEventSO.Raise(currentBullet, maxBullet);
    }

    private void ChangeFOV()
    {
        var targetFOV = isFire ? 40.0f : originFOV;
        
        // Deadzone X 값을 크게
        positionComposer.Composition.DeadZone.Size = isFire ? new Vector2(0.8f, 0.05f) : originSize;
        
        // Lerp (선형 보간) 부드럽게 FOV 조절
        cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(cinemachineCamera.Lens.FieldOfView, targetFOV, Time.deltaTime * 10.0f);
    }

    private void DetectMonster()
    {
        // Ray 표시
        Debug.DrawRay(firePos.position, firePos.forward * 100.0f, Color.red);
        if (Physics.Raycast(firePos.position, firePos.forward, out var hit, 100.0f, 1 << 8))
        {
            // Aim Dot Red
            aimDot.color = Color.red;
            aimDot.transform.localScale = Vector3.one * 2.0f;
        }
        else
        {
            // Aim Dot White
            aimDot.color = Color.white;
            aimDot.transform.localScale = Vector3.one;
        }
    }

    private IEnumerator ShowMuzzleFlash()
    {
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;
        muzzleFlash.material.mainTextureOffset = offset;

        float scale = Random.Range(1.0f, 2.5f);
        muzzleFlash.transform.localScale = Vector3.one * scale;

        float angle = Random.Range(0, 360);
        muzzleFlash.transform.localRotation = Quaternion.Euler(Vector3.forward * angle);
        
        muzzleFlash.enabled = true;

        yield return new WaitForSeconds(0.2f);
        
        muzzleFlash.enabled = false;
    }

    private void Fire()
    {
        //Instantiate(bulletPrefab, firePos.position, firePos.rotation);
        
        // Pool 총알 가져옴
        var bullet = BulletPool.Instance.pool.Get();
        
        // 총알의 위치와 방향을 설정
        bullet.transform.SetPositionAndRotation(firePos.position, firePos.rotation);
        
        // 총알 발사
        bullet.Shoot();
        
        impulseSource.GenerateImpulse();
        
        // CrossHair Animation 처리
        crossHairAnim.SetTrigger(hashFire);
        
        Debug.DrawRay(firePos.position, firePos.forward * 100.0f, Color.red);

        if (Physics.Raycast(firePos.position, firePos.forward, out var hit, 20.0f, 1 << 8 | 1 << 9))
        {
            // 몬스터와 배럴에 데미지를 추가
            hit.transform.GetComponent<IDamagable>().Damage(20);
            
            // Aim Dot Red
            aimDot.color = Color.red;
            aimDot.transform.localScale = Vector3.one * 2.0f;
        }
        else
        {
            // Aim Dot White
            aimDot.color = Color.white;
            aimDot.transform.localScale = Vector3.one;
        }
    }

    private void FireSfx()
    {
        audio.PlayOneShot(fireSfx, 0.8f);
    }
}
