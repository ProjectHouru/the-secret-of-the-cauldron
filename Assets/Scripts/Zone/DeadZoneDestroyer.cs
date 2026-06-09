using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DeadZoneDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(col.gameObject);
    }
}