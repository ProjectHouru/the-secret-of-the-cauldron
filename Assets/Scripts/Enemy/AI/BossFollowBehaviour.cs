using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(EnemyController))]
public class BossFollowBehaviour : MonoBehaviour, IEnemyBehaviour
{
    private static readonly int Agre = Animator.StringToHash("Agre");
    
    [SerializeField] private PlayerDetectZone _fightPlayerDetect;
    [SerializeField]  private ParticleSystem _fightParticleSystem;
    
    private EnemyController _enemyController;
    private Animator _animator;
    
    private void Awake()
    {
        _enemyController = GetComponent<EnemyController>();
        _animator = GetComponent<Animator>();
    }

    public void Execute()
    {
        if (_fightPlayerDetect.TryGetPlayer(out var playerController) && !_enemyController.IsAttackState)
        {
            _enemyController.SetFollowState(true);
            
            var currentPosition = transform.position;
            var goalPosition = playerController.transform.position;
            
            var goalDirection = (goalPosition - currentPosition).normalized;
         
            _enemyController.OnMoveInputHandler(goalDirection);
            
            if (_animator && !_animator.GetBool(Agre))
            {
                _animator.SetBool(Agre, true);
            }

            if (_fightParticleSystem && !_fightParticleSystem.gameObject.activeSelf)
            {
                _fightParticleSystem.gameObject.SetActive(true);
                _fightParticleSystem.Play();
            }
        }
        else if (!_enemyController.IsAttackState)
        {
            _enemyController.SetFollowState(false);
            
            if (_animator && _animator.GetBool(Agre))
            {
                _animator.SetBool(Agre, false);
            }
            
            if (_fightParticleSystem && _fightParticleSystem.gameObject.activeSelf)
            {
                _fightParticleSystem.Stop();
                _fightParticleSystem.gameObject.SetActive(false);
            }
        }
    }
}