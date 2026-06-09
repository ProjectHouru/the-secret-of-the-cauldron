using System;
using UnityEngine;

public class StartDialogueQuest: BaseQuest
{
    public override QuestId Id => QuestId.FirstMeeting;
    
    public override string Description =>
        "Поговорите с Бабулькой у  котла";
    
    protected override void DoAfterStart()
    {
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