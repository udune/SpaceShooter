using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public Color color = Color.yellow;
    [Range(0.1f, 2.0f)] 
    public float radius = 0.1f;

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
