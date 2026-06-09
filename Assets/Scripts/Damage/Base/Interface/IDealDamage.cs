public interface IDealDamage
{
    void TakeToPlayer(PlayerTakeDamage playerTakeDamage);
    void TakeToEnemy(EnemyTakeDamage enemyTakeDamage);
    void TakeToObject(ObjectTakeDamage objectTakeDamage);
}
