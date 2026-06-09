using System.Collections;
using UnityEngine;

public class PlayerAttackDealDamage : BaseColliderDealDamage
{
    private static readonly int AttackEffect = Animator.StringToHash("Attack");

    [SerializeField, Range(1f, 5f)] private int _damage;
    [SerializeField, Range(0f, 5f)] private int _damageForce;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;
    private Vector2 _direction = Vector2.zero;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void SetAttackDirection(Vector2 direction)
    {
        _direction = direction;
        
        _spriteRenderer.sortingOrder = 3;
        
        var rotation = Vector3.zero;
        var position = gameObject.transform.position;

        if (direction.y < 0)
        {
            // skip
        }
        else if (direction.y > 0)
        {
            rotation.z = 180;
            _spriteRenderer.sortingOrder = 2;
        }
        else if (direction.x < 0)
        {
            rotation.z = -90;
            position.x -= 0.5f;
            _boxCollider2D.offset = new Vector2(0, 0);
        }
        else if (direction.x > 0)
        {
            rotation.z = 90;
            position.x += 0.5f;
            _boxCollider2D.offset = new Vector2(0, 0);
        }

        gameObject.transform.rotation = Quaternion.Euler(rotation);
        gameObject.transform.position = position;
        
        Debug.Log("AttackDirection: " + direction + " rotation = " + rotation);
    }

    public void AnimateAttack()
    {
        _animator.SetTrigger(AttackEffect);
    }

    public void OnAnimationCompleteHandler()
    {
        Destroy(gameObject);
    }

    public override void TakeToPlayer(PlayerTakeDamage player)
    {
    }

    public override void TakeToEnemy(EnemyTakeDamage enemyTakeDamage)
    {
        enemyTakeDamage.HealthPoint.Decrement(_damage);
        _collider.attachedRigidbody.velocity += _direction * _damageForce;
    }

    public override void TakeToObject(ObjectTakeDamage objectTakeDamage)
    {
        objectTakeDamage.HealthPoint.Decrement(_damage);
        _collider.attachedRigidbody.velocity += _direction * _damageForce;
    }
}