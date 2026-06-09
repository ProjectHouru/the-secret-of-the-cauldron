using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Inventory: MonoBehaviour
{
    private readonly Dictionary<ItemType, InventoryItem> _items = new();
    
    private event UnityAction<InventoryItem, ICollectibleItem> _onAddItem;
    public event UnityAction<InventoryItem, ICollectibleItem> OnAddItem
    {
        add { _onAddItem -= value; _onAddItem += value; }
        remove => _onAddItem -= value;
    }

    private event UnityAction<InventoryItem> _onRemoveItem;
    public event UnityAction<InventoryItem> OnRemoveItem
    {
        add { _onRemoveItem -= value; _onRemoveItem += value; }
        remove => _onRemoveItem -= value;
    }

    public Dictionary<ItemType, InventoryItem> GetItems()
    {
        return _items;
    } 
    
    public void AddItem(ICollectibleItem item, int quantity = 1)
    {
        if (_items.ContainsKey(item.GetKey()))
        {
            _items[item.GetKey()].Quantity += quantity;
        }
        else
        {
            _items.Add(item.GetKey(), new InventoryItem(item.GetKey(), item.GetName(), quantity));
        }
        
        Debug.Log($"Inventory Add Item key={item.GetKey()} quantity={_items[item.GetKey()].Quantity}");
        
        _onAddItem?.Invoke(_items[item.GetKey()], item);
        
        //PlayerPrefs.SetInt("InventoryItem_" + item.GetKey(), _items[item.GetKey()].Quantity);
    }

    public InventoryItem GetRandomItem()
    {
        return _items.ElementAt(Random.Range(0, _items.Count)).Value;
    }

    public int GetItemQuantity(ItemType itemType)
    {
        if (HasItem(itemType))
        {
            return _items[itemType].Quantity;
        }

        return 0;
    }
    
    public bool HasItem(ItemType itemType)
    {
        return _items.ContainsKey(itemType);
    }
    
    public void RemoveItem(ItemType itemType, int quantity = 1)
    {
        if (_items.ContainsKey(itemType))
        {
            _items[itemType].Quantity -= quantity;

            Debug.Log($"Inventory Remove Item key={itemType} quantity={_items[itemType].Quantity}");

            _onRemoveItem?.Invoke(_items[itemType]);
            
            if (_items[itemType].Quantity <= 0)
            {
                _items.Remove(itemType);
            }
        }
    }
}