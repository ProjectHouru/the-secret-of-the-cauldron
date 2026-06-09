using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class IdleBehaviour : MonoBehaviour, IEnemyBehaviour
{
    private EnemyController _enemyController;

    private void Awake()
    {
        _enemyController = GetComponent<EnemyController>();
    }

    public void Execute()
    {
        if (!_enemyController.IsFollowState && !_enemyController.IsAttackState)
        {
            _enemyController.OnMoveInputHandler(Vector2.zero);
        }
    }
}