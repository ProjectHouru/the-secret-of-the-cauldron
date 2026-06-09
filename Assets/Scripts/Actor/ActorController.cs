using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ActorController : MonoBehaviour
{
    private readonly List<IActorAction> _actions = new();
    private readonly List<IUsableObjectZone> _usableObjectZones = new();
    
    protected UnityAction<Vector2> _onMoveInput;
    protected UnityAction<bool> _onAttackInput;
    protected UnityAction<bool> _onFireInput;
    protected UnityAction<UserInputActionName> _onKeyInput;
    private UnityAction _onDead;
    private UnityAction _onRespawn;
    
    protected bool _isDead;
    public bool IsDead => _isDead;
     
    public event UnityAction OnDead
    {
        add
        {
            _onDead -= value;
            _onDead += value;
        }
        remove => _onDead -= value;
    }

    public event UnityAction OnRespawn
    {
        add
        {
            _onRespawn -= value;
            _onRespawn += value;
        }
        remove => _onRespawn -= value;
    }
    
    public event UnityAction<Vector2> OnMoveInput
    {
        add
        {
            _onMoveInput -= value;
            _onMoveInput += value;
        }
        remove => _onMoveInput -= value;
    }

    public event UnityAction<bool> OnFireInput
    {
        add
        {
            _onFireInput -= value;
            _onFireInput += value;
        }
        remove => _onFireInput -= value;
    }

    public event UnityAction<bool> OnAttackInput
    {
        add
        {
            _onAttackInput -= value;
            _onAttackInput += value;
        }
        remove => _onAttackInput -= value;
    }
    
    public event UnityAction<UserInputActionName> OnKeyInput
    {
        add
        {
            _onKeyInput -= value;
            _onKeyInput += value;
        }
        remove => _onKeyInput -= value;
    }

    public void Register(IActorAction action)
    {
        if (!_actions.Contains(action))
        {
            _actions.Add(action);
        }
    }

    public void UnRegister(IActorAction action)
    {
        if (_actions.Contains(action))
        {
            _actions.Remove(action);
        }
    }

    public void Die()
    {
        _isDead = true;
        
        _onMoveInput?.Invoke(Vector2.zero);
        _onDead?.Invoke();
    }
    
    public void Respawn()
    {
        _isDead = false;
     
        _onRespawn?.Invoke();
    }
    
    public List<IUsableObjectZone> GetUsableObjectZones()
    {
        return _usableObjectZones;
    }
    
    public void AddUsableObjectZone(IUsableObjectZone usableObjectZone)
    {
        if (!_usableObjectZones.Contains(usableObjectZone))
        {
            _usableObjectZones.Add(usableObjectZone);
        }
    }

    public void RemoveUsableObjectZone(IUsableObjectZone usableObjectZone)
    {
        if (_usableObjectZones.Contains(usableObjectZone))
        {
            _usableObjectZones.Remove(usableObjectZone);
        }
    }

    private void FixedUpdate()
    {
        foreach (var action in _actions)
        {
            action.Run();
        }
    }
}