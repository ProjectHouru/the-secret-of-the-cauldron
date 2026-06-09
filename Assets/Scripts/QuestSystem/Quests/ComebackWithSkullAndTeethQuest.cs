using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComebackWithSkullAndTeethQuest : BaseQuest
{
    [SerializeField] private TeleportZone _teleport;
    [SerializeField] private int _count = 3;

    public override QuestId Id => QuestId.ComebackWithSkullAndTeeth;

    public override string Description => 
        "Вернитесь к Бабульке\n" +
        "Хрустящий Череп " + (GameLogic.Instance.PlayerInventory.GetItemQuantity(ItemType.Skull) > 0 ? "<color=green>найден</color>" : "<color=red>не найден</color>") + "!\n" +
        "Собрано тыкв <color=orange>" + GameLogic.Instance.PlayerInventory.GetItemQuantity(ItemType.Teeth) + " из " + _count + "</color>";

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
