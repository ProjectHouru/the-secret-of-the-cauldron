public interface IUsableItem
{
    public bool CanBeUsed();
    
    public void Use(ActorController actorController);
    
    public void CantUse(ActorController actorController);
}