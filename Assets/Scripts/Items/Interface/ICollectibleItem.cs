using UnityEngine;

public interface ICollectibleItem
{
    public ItemType GetKey();
    
    public string GetName();
}