using UnityEngine;
using UnityEngine.Events;

public abstract class BaseUsableItem: MonoBehaviour, IUsableItem
{
    private event UnityAction _onUse;
    public event UnityAction OnUse
    {
        add { _onUse -= value; _onUse += value; }
        remove => _onUse -= value;
    }
    
    private event UnityAction _onCantUse;
    public event UnityAction OnCantUse
    {
        add { _onCantUse -= value; _onCantUse += value; }
        remove => _onCantUse -= value;
    }
    
    public bool CanBeUsed()
    {
        return true;
    }

    public virtual void Use(ActorController actorController)
    {
        _onUse?.Invoke();
    }

    public virtual void CantUse(ActorController actorController)
    {
        _onCantUse?.Invoke();
    }
}