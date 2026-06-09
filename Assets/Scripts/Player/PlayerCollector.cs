using UnityEngine;

[RequireComponent (typeof(Inventory))]
public class PlayerCollector : MonoBehaviour, ICollector
{
    private Inventory _inventory;
    
    private void Awake()
    {
        _inventory = GetComponent<Inventory>();
    }

    public bool Collect(ICollectibleItem collectibleItem)
    {
        _inventory.AddItem(collectibleItem);

        return true;
    }
}