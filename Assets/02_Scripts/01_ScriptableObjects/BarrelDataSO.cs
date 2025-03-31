using UnityEngine;

[CreateAssetMenu(fileName = "BarrelDataSO", menuName = "Scriptable Objects/BarrelDataSO")]
public class BarrelDataSO : ScriptableObject
{
    public GameObject explosionPrefab;
    public AudioClip expAudioClip;
}
