using System;
using UnityEngine;

public class CollectSkullAndTeethQuest: BaseQuest
{
    [SerializeField] private TeleportZone _teleport;
    [SerializeField] private int _count = 3;
    
    public override QuestId Id => QuestId.CollectSkullAndTeeth;
    
    public override string Description => 
        "Найдите Хрустящий Череп и тыквы на Кладбище (налево от Бабульки)\n" +
        "Хрустящий Череп " + (GameLogic.Instance.PlayerInventory.GetItemQuantity(ItemType.Skull) > 0 ? "<color=green>найден</color>" : "<color=red>не найден</color>") + "!\n" +
        "Собрано тыкв <color=orange>" + GameLogic.Instance.PlayerInventory.GetItemQuantity(ItemType.Teeth) + " из " + _count + "</color>";

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
        if (item.Type == ItemType.Skull || item.Type == ItemType.Teeth)
        {
            UpdateQuest();
            
            // Если собрали привязанный к боссу предмет
            if (collectibleItem.GetName().IndexOf("[BOSS]") != -1)
            {
                BossActivate();
            }
            
            // Если собрали все предметы
            if (
                GameLogic.Instance.PlayerInventory.GetItemQuantity(ItemType.Skull) > 0 
                && GameLogic.Instance.PlayerInventory.GetItemQuantity(ItemType.Teeth) >= _count)
            {
                // Отвязываемся от события
                GameLogic.Instance.PlayerInventory.OnAddItem -= OnAddItem;
                
                CompleteQuest();
            }
        }
    }

    private void BossActivate()
    {
        GameLogic.Instance.PumpkinBoss.gameObject.SetActive(true);
        
        GameUI.Instance.ShowNoty("Бить бесполезно! Бегите!", 10f);

        GameLogic.Instance.PlayBossMusic();
    }
}