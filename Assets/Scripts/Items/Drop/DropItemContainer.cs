using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class DropItemContainer: MonoBehaviour
{
    [SerializeField] private float _removeTimer = 0.7f;
    
    private CollectableItem _innerItem;
    private CircleCollider2D _circleCollider2D;
    
    private bool _wasGrounded;

    private void Awake()
    {
        _circleCollider2D = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        StartCoroutine(RemoveContainerTimer());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(RemoveContainerTimer());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void AddInnerItem(GameObject itemPrefab)
    {
        // Create item
        var item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
        item.transform.SetParent(transform, false);
        
        // Save to property
        _innerItem = item.GetComponent<CollectableItem>();
        
        // Sync collider bounds
        SyncColliders(item);
        
        // Stop item animation
        _innerItem.StopAnimation();
    }

    private void SyncColliders(GameObject item)
    {
        var xSize = item.GetComponent<Collider2D>().bounds.size.x;
        
        _circleCollider2D.radius = xSize / 2;
    }

    private IEnumerator RemoveContainerTimer()
    {
        yield return new WaitForSeconds(_removeTimer);
        
        RemoveContainer();
    }

    private void RemoveContainer()
    {
        if (_innerItem)
        {
            _innerItem.transform.SetParent(null);
            _innerItem.StartAnimation();
        }
        
        Destroy(gameObject);
    }
}