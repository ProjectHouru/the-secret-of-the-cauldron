using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComebackWithBellsQuest : BaseQuest
{
    [SerializeField] private TeleportZone _teleport;
    [SerializeField] private int _count = 7;

    public override QuestId Id => QuestId.ComebackWithBells;

    public override string Description =>
        "Вернитесь к Бабульке\n"
        + "Собрано цветов <color=purple>" + GameLogic.Instance.PlayerInventory.GetItemQuantity(ItemType.PoisonBells) + " из " + _count + "</color>";

    protected override void DoAfterStart()
    {
        // Активация обратного телепорта
        _teleport.Activate();
        
        // Активация зоны действия Бабульки
        GameLogic.Instance.WitchController.GetComponent<WitchUsableZone>().Activate();
        
        // Активируем остановку бабули, если подошел персонаж
        GameLogic.Instance.WitchController.GetComponent<StopBehaviour>().Activate();
    }

    public override void TriggerQuest()
    {
        // Удаляем предмет из инвентаря
        GameLogic.Instance.PlayerInventory.RemoveItem(ItemType.PoisonBells, GameLogic.Instance.PlayerInventory.GetItemQuantity(ItemType.PoisonBells));
        
        // Останавливаем игрока
        GameLogic.Instance.DialogueSystem.OnStart += GameLogic.Instance.StopPlayer;
        
        // Завершаем квест
        GameLogic.Instance.DialogueSystem.OnEnd += CompleteQuest;
        
        // Вызываем диалог
        GameLogic.Instance.DialogueSystem.StartDialogue(DialogueBase.Instance.GetDialogue(Id));
    }
    
    protected override void DoAfterComplete()
    {
        // Деактивируем остановку бабули, если подошел персонаж
        GameLogic.Instance.WitchController.GetComponent<StopBehaviour>().Deactivate();
        
        // Запускаем игрока
        GameLogic.Instance.StartPlayer();
    }
}
