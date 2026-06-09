using UnityEngine;
using UnityEngine.Events;

public class BaseUsableObjectZone: MonoBehaviour, IUsableObjectZone
{
    private event UnityAction _onUse;
    public event UnityAction OnUse
    {
        add { _onUse -= value; _onUse += value; }
        remove => _onUse -= value;
    }
    
    public virtual bool CanBeUsed()
    {
        return true;
    }

    public virtual void Use()
    {
        _onUse?.Invoke();
    }

    public virtual void StayDetect (ActorController actorController)
    {
    }

    public virtual void Detect(ActorController actorController)
    {
    }

    public virtual void Miss()
    {
    }
}