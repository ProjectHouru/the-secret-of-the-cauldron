using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HealthPoint))]
public class OnTakeDamage : MonoBehaviour
{
    private static readonly int TakeDamage = Animator.StringToHash("Hit");
    private static readonly int TakeDeadlyDamage = Animator.StringToHash("Hit");

    [Header("Die Effect")]
    [SerializeField] private GameObject _deadEffectPrefab;
    [SerializeField] private bool _inheritForceForDeadEffect = false;
    [SerializeField, Range(0f, 5f)] private float _destroyTimer;
    
    [Header("Drop Spawner")]
    [SerializeField]  private DropSpawner _onTakeDamageDropSpawner;
    [SerializeField]  private DropSpawner _onDeadDropSpawner;
    
    [Header("Sound Effect")]
    [SerializeField] private SoundType _damageSound = SoundType.None;
    [SerializeField] private SoundType _dieSound = SoundType.None;
    
    protected HealthPoint _healthPoint;
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    
    protected virtual void Awake()
    {
        _healthPoint = GetComponent<HealthPoint>();
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected void OnEnable()
    {
        _healthPoint.OnDecreaseValue += DecreaseValueHandler;
    }

    protected void OnDisable()
    {
        _healthPoint.OnDecreaseValue -= DecreaseValueHandler;
    }

    private bool ContainsAnimationTrigger(int animationTriggerHash)
    {
        if (_animator != null)
        {
            foreach (AnimatorControllerParameter param in _animator.parameters)
            {
                if (param.nameHash == animationTriggerHash)
                {
                    return true;
                }
            }
        }

        return false;
    }

    protected virtual void DecreaseValueHandler()
    {
        Debug.Log(gameObject.name + " HP: " + _healthPoint.CurrentValue);
        
        if (_healthPoint.IsAlive)
        {
            // Animation
            if (ContainsAnimationTrigger(TakeDamage))
            {
                _animator.SetTrigger(TakeDamage);
            }

            // Sound
            if (_damageSound != SoundType.None)
            {
                SoundManager.Instance.PlaySound(_damageSound, gameObject);
            }
            
            // Drop Spawn
            DropItemsAfterDamage();
        }
        else
        {
            // Animation
            if (ContainsAnimationTrigger(TakeDeadlyDamage))
            {
                _animator.SetTrigger(TakeDeadlyDamage);
            }

            // Sound
            if (_dieSound != SoundType.None)
            {
                SoundManager.Instance.PlaySound(_dieSound, gameObject);
            }
            else if (_damageSound != SoundType.None)
            {
                SoundManager.Instance.PlaySound(_damageSound, gameObject);
            }

            // Drop Spawn
            DropItemsAfterDead();

            if (_destroyTimer > 0)
            {
                // Die Effect
                StartCoroutine(DieEffectTimer());
            }
        }
    }

    
    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator DieEffectTimer()
    {
        yield return new WaitForSeconds(_destroyTimer);
        
        DieEffect(); 
    }
    
    private void DropItemsAfterDamage()
    {
        if (_onTakeDamageDropSpawner)
        {
            _onTakeDamageDropSpawner.Spawn();
        }
    }
    
    private void DropItemsAfterDead()
    {
        if (_onDeadDropSpawner)
        {
            _onDeadDropSpawner.Spawn();
        }
    }
    
    private void DieEffect()
    {
        if (_deadEffectPrefab != null)
        {
            var effectPrefab = Instantiate(_deadEffectPrefab, transform.position, Quaternion.identity);

            // Velocity transfer for children
            if (_inheritForceForDeadEffect)
            {
                var rigidbodyChildren = effectPrefab.GetComponentsInChildren<Rigidbody2D>();
                
                foreach (var rb in rigidbodyChildren)
                {
                    rb.velocity = _rigidbody2D.velocity * Random.Range(1f, 2f);
                }
            }
        }

        DestroyObject();
    }

    private void DestroyObject()
    {
        gameObject.SetActive(false);
    }
}