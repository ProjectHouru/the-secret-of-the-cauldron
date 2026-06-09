public class OnActorTakeDamage : OnTakeDamage
{ 
    private ActorController _actorController;
    
    protected override void Awake()
    {
        base.Awake();
        
        _actorController = GetComponent<ActorController>();
    }

    protected override void DecreaseValueHandler()
    {
        base.DecreaseValueHandler();
        
        if (!_healthPoint.IsAlive)
        {
            // Disable Controller
            _actorController.Die();
        }
    }
}