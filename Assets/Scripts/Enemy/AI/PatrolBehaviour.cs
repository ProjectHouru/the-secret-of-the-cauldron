using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class PatrolBehaviour : MonoBehaviour, IEnemyBehaviour
{
    [SerializeField] private PatrolZone _patrolZone;
    
    private EnemyController _enemyController;

    private void Awake()
    {
        _enemyController = GetComponent<EnemyController>();
    }

    public void Execute()
    {
        if (!_enemyController.IsAttackState && !_enemyController.IsFollowState)
        {
            var enemyPosition = transform.position;
            var boundPosition = _patrolZone.GetTarget();

            if (boundPosition != Vector3.zero)
            {
                var runDirection = (boundPosition - enemyPosition).normalized;

                _enemyController.OnMoveInputHandler(runDirection);
                
                if (Vector2.Distance(boundPosition, enemyPosition) < 0.1)
                {
                    _patrolZone.Next();
                }
            }
        }
    }
}