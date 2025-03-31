using UnityEngine;

[CreateAssetMenu(fileName = "SpiderDataSO", menuName = "Scriptable Objects/SpiderDataSO")]
public class SpiderDataSO : ScriptableObject
{
    public float attackDistance = 3.0f;
    public float traceDistance = 10.0f;
}
