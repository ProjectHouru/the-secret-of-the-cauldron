public interface IUsableObjectZone
{
    public bool CanBeUsed();
    
    public void Use();

    public void Detect(ActorController actorController);
    
    public void Miss();
}