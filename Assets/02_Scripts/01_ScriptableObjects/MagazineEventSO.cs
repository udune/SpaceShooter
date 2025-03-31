using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MagazineEventSO", menuName = "Scriptable Objects/MagazineEventSO")]
public class MagazineEventSO : ScriptableObject
{
    private event Action<int, int> listeners;

    public void Subscribe(Action<int, int> listener)
    {
        listeners += listener;
    }

    public void Unsubscribe(Action<int, int> listener)
    {
        listeners -= listener;
    }

    public void Raise(int currentBullet, int maxBullet)
    {
        listeners?.Invoke(currentBullet, maxBullet);
    }
}
