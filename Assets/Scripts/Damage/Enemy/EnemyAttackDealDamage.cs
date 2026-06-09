using System.Collections;
using UnityEngine;

public class EnemyAttackDealDamage : BaseColliderDealDamage
{
    private static readonly int AttackEffect = Animator.StringToHash("Attack");

    [SerializeField, Range(0f, 5f)] private int _damage;
    [SerializeField, Range(0f, 20)] private int _damageForce;

    private Animator _animator;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void AnimateAttack()
    {
        _animator.SetTrigger(AttackEffect);
    }
    
    public void OnAnimationCompleteHandler()
    {
        Destroy(gameObject);
    }
    
    /// <summary>
    /// Наносим урон игроку
    /// </summary>
    /// <param name="player"></param>
    public override void TakeToPlayer(PlayerTakeDamage player)
    {
        player.HealthPoint.Decrement(_damage);
        
        var direction = (player.gameObject.transform.position - gameObject.transform.position).normalized.ToVector2();
        
        Debug.Log("Attack Direction = " + direction);
        
        _collider.attachedRigidbody.AddForce(direction * _damageForce, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Не наносим урон другим врагам
    /// </summary>
    /// <param name="enemyTakeDamage"></param>
    /// <exception cref="NotImplementedException"></exception>
    public override void TakeToEnemy(EnemyTakeDamage enemyTakeDamage)
    {
    }

    public override void TakeToObject(ObjectTakeDamage objectTakeDamage)
    {
    }
}