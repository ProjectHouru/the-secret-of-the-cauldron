using System;
using UnityEngine;

public class OnboardingCollectingQuest : BaseQuest
{
    [SerializeField] private int _count = 1;

    public override QuestId Id => QuestId.OnboardingCollecting;

    public override string Description =>
        "Обучение (взаимодействие с предметами)\n" +
        "Идите дальше по дороге и подберите меч";

    protected override void DoAfterStart()
    {
        GameUI.Instance.ShowNoty("Для взаимодействия с предметами и персонажами встаньте рядом с ними и используйте клавишу E на клавиатуре", 5f);
        GameLogic.Instance.PlayerInventory.OnAddItem += OnAddItem;
    }

    protected override void DoAfterComplete()
    {
        GameLogic.Instance.PlayerController.StopFire = false;
        GameUI.Instance.HideNoty();
    }

    private void OnAddItem(InventoryItem item, ICollectibleItem collectibleItem)
    {
        UpdateQuest();

        if (item.Type == ItemType.Sword && item.Quantity >= _count)
        {
            CompleteQuest();
        }
    }
}