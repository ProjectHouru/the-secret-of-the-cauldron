using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyController))]
public class WitchPatrolBehaviour : MonoBehaviour, IEnemyBehaviour
{
    [SerializeField] private float _delayTime = 2f;
    [SerializeField] private PatrolZone _patrolZone;

    private bool _isDelay = false;
    private EnemyController _enemyController;
    private SpriteRenderer _renderer;
    private Transform _forceTarget;
    private bool _isActive = true;
    
    private event UnityAction _onTargetReached;

    public event UnityAction OnTargetReached
    {
        add
        {
            _onTargetReached += value;
        }
        remove => _onTargetReached -= value;
    }
    
    private void Awake()
    {
        _enemyController = GetComponent<EnemyController>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void Deactivate()
    {
        _isActive = false;
    }
    
    public void Activate()
    {
        _isActive = true;
    }
    
    public void SetTarget(Transform _target)
    {
        _forceTarget = _target;
    }
    
    public void Execute()
    {
        if (!_enemyController.IsAttackState && !_enemyController.IsFollowState && !_isDelay && _isActive)
        {
            var enemyPosition = transform.position;
            var boundPosition = _forceTarget ? _forceTarget.position : _patrolZone.GetTarget();

            if (boundPosition != Vector3.zero)
            {
                var runDirection = (boundPosition - enemyPosition).normalized;

                _enemyController.OnMoveInputHandler(runDirection);
                
                if (Vector2.Distance(boundPosition, enemyPosition) < 0.1)
                {
                    _enemyController.OnMoveInputHandler(Vector2.down);
                    _enemyController.OnMoveInputHandler(Vector2.zero);
                    
                    _onTargetReached?.Invoke();

                    if (_forceTarget)
                    {
                        _forceTarget = null;
                    }
                    else
                    {
                        _patrolZone.Next();

                        _isDelay = true;

                        if (_delayTime > 0 && _renderer.isVisible)
                        {
                            ShowDialogueMessage();
                        }

                        StartCoroutine(Delay());
                    }
                }
            }
        }
    }

    private void ShowDialogueMessage()
    {
        if (GameLogic.Instance.DialogueSystem)
        {
            GameLogic.Instance.DialogueSystem.OneMessage(new DialogueMessage(EDialogueActor.Witch, DialogueBase.Instance.GetWitchMessage()));
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(_delayTime);

        _isDelay = false;
    }
}