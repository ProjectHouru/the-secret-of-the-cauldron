using UnityEngine;
using System.Collections;

public class EnemyAttackAction : MonoBehaviour, IActorAction
{
     private static readonly int Attack = Animator.StringToHash("Attack");
     
     [SerializeField] private EnemyAttackDealDamage _attackPrefab;
     [SerializeField] private Transform _attackSpawnPoint;
     
    [SerializeField, Range(0f, 10f)] private float _attackCooldown;
    
    [SerializeField] private SoundType _attackSound = SoundType.None;

    private Animator _animator;
    
    private bool _desiredAttack;
    private bool _canAttack = true;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    private void Start()
    {
        var actorController = GetComponent<ActorController>();
        actorController.Register(this);
        actorController.OnAttackInput += OnAttackInputHandler;
    }
    
    private void OnAttackInputHandler(bool input)
    {
        _desiredAttack |= input;
    }

    public void Run()
    {
        if (_desiredAttack)
        {
            _desiredAttack = false;

            if (_canAttack)
            {
                // Атакуем
                DoAttack();

                // Анимируем атаку
                AnimateAttack();

                // Запускаем кулдаун атаки
                StartCoroutine(AttackCooldown());
            }
        }
    }
    
    private void DoAttack()
    {
        // Создаем атаку
        var attackObject = Instantiate(_attackPrefab.gameObject, Vector3.zero, Quaternion.identity);
        attackObject.transform.SetParent(_attackSpawnPoint, false);
        
        // Задаем направление и анимацию
        var attackSwordDealDamage = attackObject.GetComponent<EnemyAttackDealDamage>();
        attackSwordDealDamage.AnimateAttack();
        
        // Sound
        SoundManager.Instance.PlaySound(_attackSound, gameObject);
    }
    
    protected virtual void AnimateAttack()
    {
        if (_animator != null)
        {
            _animator.SetTrigger(Attack);
        }
    }
    
    private IEnumerator AttackCooldown()
    {
        _canAttack = false;
        
        yield return new WaitForSeconds(_attackCooldown);

        _canAttack = true;
    }
}