using System;
using UnityEngine;

public class BringSkullToCauldron: BaseQuest
{
    [SerializeField] private GameObject _theCake;
    
    public override QuestId Id => QuestId.BringSkullToCauldron;

    public override string Description =>
        "Брось череп в котел";
      

    protected override void DoAfterStart()
    {
        GameUI.Instance.ShowNoty("Подойдите к котлу и нажмите клавишу \"E\" на клавиатуре", 5f);
        
        // Активируем остановку бабули, если подошел персонаж
        GameLogic.Instance.WitchController.GetComponent<StopBehaviour>().Activate();
        
        // Активация зоны действия котла
        GameLogic.Instance.WitchCauldron.Activate();
    }

    public override void TriggerQuest()
    {
        // Удаляем предмет из инвентаря
        GameLogic.Instance.PlayerInventory.RemoveItem(ItemType.Skull, GameLogic.Instance.PlayerInventory.GetItemQuantity(ItemType.Skull));
        
        // Останавливаем игрока
        GameLogic.Instance.DialogueSystem.OnStart += GameLogic.Instance.StopPlayer;
        
        // Завершаем диалог 1
        GameLogic.Instance.DialogueSystem.OnEnd += CompleteDialog1;
        
       // Начинаем диалог
        GameLogic.Instance.DialogueSystem.StartDialogue(DialogueBase.Instance.GetDialogue(Id));
    }
    
    private void CompleteDialog1()
    {
        // Показываем торт
        _theCake.SetActive(true);
        
        // Завершаем квест
        GameLogic.Instance.DialogueSystem.OnStart += () => { };
        GameLogic.Instance.DialogueSystem.OnEnd += CompleteQuest;
        
        // Вызываем диалог
        GameLogic.Instance.DialogueSystem.StartDialogue(DialogueBase.Instance.GetDialogue(QuestId.TheCake_OnlyDialog));
    }
    
    protected override void DoAfterComplete()
    {
        // Запускаем игрока
        GameLogic.Instance.StartPlayer();
        
        // Деактивируем остановку бабули, если подошел персонаж
        GameLogic.Instance.WitchController.GetComponent<StopBehaviour>().Deactivate();
    }
}