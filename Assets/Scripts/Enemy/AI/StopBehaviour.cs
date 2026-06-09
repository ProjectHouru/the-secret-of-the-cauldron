using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class StopBehaviour : MonoBehaviour, IEnemyBehaviour
{
    [SerializeField] private PlayerDetectZone _playerDetect;
    
    private EnemyController _enemyController;
    private bool _isActive = true;

    public void Deactivate()
    {
        _isActive = false;
    }
    
    public void Activate()
    {
        _isActive = true;
    }

    private void Awake()
    {
        _enemyController = GetComponent<EnemyController>();
    }

    public void Execute()
    {
        if (_playerDetect.TryGetPlayer(out var playerController) && _isActive)
        {
            _enemyController.SetFollowState(true);
            
            _enemyController.OnMoveInputHandler(Vector2.down);
            _enemyController.OnMoveInputHandler(Vector2.zero);
        }
        else
        {
            _enemyController.SetFollowState(false);
        }
    }
}