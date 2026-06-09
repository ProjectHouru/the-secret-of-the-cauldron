using UnityEngine;

public class EnemyShotDealDamage : BaseShotDealDamage
{
    [SerializeField] private int _damageToPlayer;
    [SerializeField] private int _damageToObject;

    public override void TakeToPlayer(PlayerTakeDamage player)
    {
        player.HealthPoint.Decrement(_damageToPlayer);

        SelfDestroy();
    }

    public override void TakeToEnemy(EnemyTakeDamage enemy)
    {
        SelfDestroy();
    }

    public override void TakeToObject(ObjectTakeDamage objectTakeDamage)
    {
        objectTakeDamage.HealthPoint.Decrement(_damageToObject);
        SelfDestroy();
    }
}