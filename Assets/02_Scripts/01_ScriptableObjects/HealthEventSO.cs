using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthEventSO", menuName = "Scriptable Objects/HealthEventSO")]
public class HealthEventSO : ScriptableObject
{
    // 구독자를 저장할 리스너
    private event Action<float> listeners;

    // 구독자 추가
    public void Subscribe(Action<float> listener)
    {
        listeners += listener;
    }

    // 구독자 제거
    public void Unsubscribe(Action<float> listener)
    {
        listeners -= listener;
    }

    // 이벤트를 발생
    public void Raise(float hp)
    {
        listeners?.Invoke(hp);
    }
}
