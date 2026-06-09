using System;
using UnityEngine;

public class CollectPoisonBellsQuest: BaseQuest
{
    [SerializeField] private TeleportZone _teleport;
    [SerializeField] private int _count = 7;
    
    public override QuestId Id => QuestId.CollectPoisonBells;

    public override string Description =>
        "Соберите ядовитые колокольчики на болотах\n" +
        "Идите от Бабульки направо к болотам\n" +
        "Собрано цветов <color=purple>" + GameLogic.Instance.PlayerInventory.GetItemQuantity(ItemType.PoisonBells) + " из " + _count + "</color>";
      

    protected override void DoAfterStart()
    {
        _teleport.Activate();
        GameLogic.Instance.PlayerInventory.OnAddItem += OnAddItem;
    }
    
    protected override void DoAfterComplete()
    {
    }
   
    private void OnAddItem(InventoryItem item, ICollectibleItem collectibleItem)
    {
        if (item.Type == ItemType.PoisonBells)
        {
            UpdateQuest();
            
            if (item.Quantity >= _count)
            {
                // Отвязываемся от события
                GameLogic.Instance.PlayerInventory.OnAddItem -= OnAddItem;
            
                CompleteQuest();
            }
        }
    }
}