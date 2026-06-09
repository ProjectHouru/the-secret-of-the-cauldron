using UnityEngine;

public class EnemyController : ActorController
{
    public bool IsAttackState { get; private set; } = false;
    public bool IsFollowState { get; private set; } = false;
    
    public bool StopMovement = false;
    public bool StopFire = false;
    public bool StopAttack = false;

    public void SetFollowState(bool state)
    {
        IsFollowState = state;
    }
    
    public void SetAttackState(bool state)
    {
        IsAttackState = state;
    }
    
    public void OnMoveInputHandler(Vector2 value)
    {
        if (!_isDead && !StopMovement)
        {
            _onMoveInput?.Invoke(value);
        }
    }

    public void OnFireInputHandler()
    {
        if (!_isDead && !StopFire)
        {
            _onFireInput?.Invoke(true);
        }
    }
    
    public void OnAttackInputHandler()
    {
        if (!_isDead && !StopAttack)
        {
            _onAttackInput?.Invoke(true);
        }
    }

    public void Stop()
    {
        _onMoveInput?.Invoke(Vector2.zero);
        
        StopMovement = true;
        StopAttack = true;
        StopFire = true;
    }

    public void Run()
    {
        StopMovement = false;
        StopAttack = false;
        StopFire = false;
    }
}