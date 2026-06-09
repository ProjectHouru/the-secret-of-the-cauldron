using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class CollectableItem: MonoBehaviour, ICollectibleItem
{
    [Header("Main Required Params")]
    [SerializeField] private ItemType _itemType;
    [SerializeField] private string _itemName;
    
    [Header("Effects")]
    [SerializeField] private GameObject _collectEffectPrefab;
    [SerializeField] private SoundType _collectEffectSound = SoundType.None;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent<ICollector>(out var collector))
        {
            if (collector.Collect(this))
            {
                PlayCollectEffect();
                PlaySoundEffect();
                Destroy(gameObject);
            }
        }
    }

    public ItemType GetKey()
    {
        return _itemType;
    }
    
    public string GetName()
    {
        return _itemName;
    }

    public void StopAnimation()
    {
        if (_animator != null)
        {
            _animator.enabled = false;
        }
    }

    public void StartAnimation()
    {
        if (_animator != null)
        {
            _animator.enabled = true;
        }
    }

    private void PlayCollectEffect()
    {
        if (_collectEffectPrefab != null)
        {
            Instantiate(_collectEffectPrefab, transform.position, Quaternion.identity);
        }
    }
    
    private void PlaySoundEffect()
    {
        SoundManager.Instance.PlaySound(_collectEffectSound);
    }
}