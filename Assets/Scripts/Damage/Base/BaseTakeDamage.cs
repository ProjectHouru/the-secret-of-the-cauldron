using UnityEngine;

[RequireComponent(typeof(HealthPoint))]
public abstract class BaseTakeDamage : MonoBehaviour, ITakeDamage
{
    public abstract void Accept(IDealDamage visitor);

}