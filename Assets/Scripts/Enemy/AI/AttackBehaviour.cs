using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class AttackBehaviour : MonoBehaviour, IEnemyBehaviour
{
    [SerializeField] private PlayerDetectZone _playerDetect;
    
    private EnemyController _enemyController;

    private void Awake()
    {
        _enemyController = GetComponent<EnemyController>();
    }

    public void Execute()
    {
        if (_playerDetect.TryGetPlayer(out var playerController))
        {
            _enemyController.SetAttackState(true);

            _enemyController.OnMoveInputHandler(Vector2.zero);
            
            _enemyController.OnAttackInputHandler();
        }
        else
        {
            _enemyController.SetAttackState(false);
        }
    }
}