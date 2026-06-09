using System.Collections;
using UnityEngine;

public class SelfDestroyTimer : MonoBehaviour
{
    [SerializeField] private float _destroyTimer;

    private void Start()
    {
        StartCoroutine(DestroyTimerCoroutine());
    }

    private IEnumerator DestroyTimerCoroutine()
    {
        yield return new WaitForSeconds(_destroyTimer);

        Destroy(gameObject);
    }
}