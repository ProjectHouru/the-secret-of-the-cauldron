using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingFightOneQuest : BaseQuest
{
    [SerializeField] private int _count = 1;

    public override QuestId Id => QuestId.OnboardingFightOne;

    public override string Description =>
        "Обучение (основы боя)\n" +
        "Разбейте вазу на своём пути, нажмите пробел для атаки";

    protected override void DoAfterStart()
    {
        GameLogic.Instance.PlayerInventory.OnAddItem += OnAddItem;
    }

    protected override void DoAfterComplete()
    {
    }

    private void OnAddItem(InventoryItem item, ICollectibleItem collectibleItem)
    {
        if (item.Type == ItemType.VaseSoul)
        {
            UpdateQuest();
            
            if (item.Quantity >= _count)
            {
                // Отвязываемся от события
                GameLogic.Instance.PlayerInventory.OnAddItem -= OnAddItem;
                
                // Обнуляем уже разбитые вазы
                GameLogic.Instance.PlayerInventory.RemoveItem(ItemType.VaseSoul, GameLogic.Instance.PlayerInventory.GetItemQuantity(ItemType.VaseSoul));
            
                CompleteQuest();
            }
        }
    }
}