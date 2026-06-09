using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingFightTwoQuest : BaseQuest
{
    [SerializeField] private int _count = 3;
    
    public override QuestId Id => QuestId.OnboardingFightTwo;

    public override string Description =>
        "Обучение (бой с несколькими противниками)\n" +
        "Разбейте несколько глиняных кувшинов или ящиков на поляне\n" +
        "Разбито <color=#a52a2aff>" + GameLogic.Instance.PlayerInventory.GetItemQuantity(ItemType.VaseSoul) + " из " + _count + "</color>";

    protected override void DoAfterStart()
    {
        // Привязываемся к события
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
            
                CompleteQuest();
            }
        }
    }
}