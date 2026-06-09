using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class UserInputAction: MonoBehaviour, IActorAction
{
    [SerializeField] private UserInputActionName _actionName;
    [SerializeField] private ItemType _usableItemType;
    [SerializeField] bool _isInventoryReducible;
    
    // Ссылка на объект заглушку, с которого будут вызывать скрипты
    [SerializeField] private BaseUsableItem _usableItem;
    
    // Используется только для получения типа проверяемой зоны
    [SerializeField] private BaseUsableObjectZone _usableObjectZone;
   
    private Inventory _inventory;
    private ActorController _actorController;
    
    private void Awake()
    {
        _inventory = GetComponent<Inventory>();
    }

    private void Start()
    {
        _actorController = GetComponent<ActorController>();
        _actorController.Register(this);
        _actorController.OnKeyInput += OnKeyInput;
    }
    
    public void Run()
    {
    }
    
    private void OnKeyInput(UserInputActionName actionName)
    {
        if (actionName == _actionName)
        {
            if (_usableObjectZone != null)
            {
                TryUseZone();
            }
            else if (_usableItem != null && _usableItemType != ItemType.None)
            {
                TryUseItem();
            }
        }
    }

    private void TryUseZone()
    {
        var usableObjectsZones = _actorController.GetUsableObjectZones();
                
        foreach (var usableObjectZone in usableObjectsZones)
        {
            if (usableObjectZone.GetType() == _usableObjectZone.GetType() && usableObjectZone.CanBeUsed())
            {
                if (_inventory.HasItem(_usableItemType) || _usableItemType == ItemType.None)
                {
                    usableObjectZone.Use();

                    if (_usableItem != null)
                    {
                        _usableItem.Use(_actorController);
                    }

                    if (_usableItemType != ItemType.None && _isInventoryReducible)
                    {
                        _inventory.RemoveItem(_usableItemType);
                    }
                }
                else
                {
                    if (_usableItem != null)
                    {
                        _usableItem.CantUse(_actorController);
                    }
                }
            }
        }
    }

    private void TryUseItem()
    {
        if (_inventory.HasItem(_usableItemType))
        {
            if (_isInventoryReducible)
            {
                _inventory.RemoveItem(_usableItemType);
            }

            _usableItem.Use(_actorController);
        }
        else
        {
            _usableItem.CantUse(_actorController);
        }
    }
}