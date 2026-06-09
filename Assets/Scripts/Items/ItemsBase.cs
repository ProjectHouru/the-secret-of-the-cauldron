using System.Collections.Generic;
using UnityEngine;

public class ItemsBase: Singleton<ItemsBase>
{
    [SerializeField] private List<CollectableItem> _items;

    public ICollectibleItem GetItem(ItemType itemType)
    {
        var item = _items.Find(item => item.GetKey() == itemType);

        return item ? item : null;
    }
    
    public GameObject GetItemPrefab(ItemType itemType)
    {
        var item = _items.Find(item => item.GetKey() == itemType);

        return item ? item.gameObject : null;
    }
}