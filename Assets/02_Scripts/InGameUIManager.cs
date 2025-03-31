using System;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] private Image hpBar;
    [SerializeField] private Image magazine;
    
    [SerializeField] private HealthEventSO healthEventSO;
    [SerializeField] private MagazineEventSO magazineEventSO;
    
    private int maxBullet = 10;

    private void Start()
    {
        var UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
        GetComponent<Canvas>().worldCamera = UICamera;
    }

    // 이벤트 수신
    private void OnEnable()
    {
        healthEventSO.Subscribe(OnHpChanged);
        magazineEventSO.Subscribe(OnMagazineChanged);
    }

    // 이벤트 해지
    private void OnDisable()
    {
        healthEventSO.Unsubscribe(OnHpChanged);
        magazineEventSO.Unsubscribe(OnMagazineChanged);
    }

    private void OnHpChanged(float hp)
    {
        hpBar.fillAmount = hp / 100.0f;
    }

    private void OnMagazineChanged(int currentBullet, int maxBullet)
    {
        magazine.fillAmount = (float) currentBullet / maxBullet;
    }
}
