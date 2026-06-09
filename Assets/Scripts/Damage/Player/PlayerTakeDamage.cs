using UnityEngine;

[RequireComponent(typeof(HealthPoint))]
public class PlayerTakeDamage : BaseTakeDamage
{
    private HealthPoint _healthPoint;
    
    public HealthPoint HealthPoint => _healthPoint;

    private void Awake()
    {
        _healthPoint = GetComponent<HealthPoint>();
    }

    public override void Accept(IDealDamage visitor)
    {
        visitor.TakeToPlayer(this);
    }
}