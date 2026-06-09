using System;
using UnityEngine;

public class BossFightFinishCast : BaseQuest
{
    public override QuestId Id => QuestId.BossFightFinishCast;

    public override string Description =>
        "Подбегите и поговорите с Бабулькой!";

    protected override void DoAfterStart()
    {
        // Активация зоны действия Бабульки
        GameLogic.Instance.WitchController.GetComponent<WitchUsableZone>().Activate();
        
        // Активируем остановку бабули, если подошел персонаж
        GameLogic.Instance.WitchController.GetComponent<StopBehaviour>().Activate();
    }

    public override void TriggerQuest()
    {
        // Останавливаем босса
        GameLogic.Instance.PumpkinBoss.Stop();
        
        // Останавливаем игрока
        GameLogic.Instance.DialogueSystem.OnStart += GameLogic.Instance.StopPlayer;
        
        // Завершаем диалог 1
        GameLogic.Instance.DialogueSystem.OnEnd += CompleteDialog1;
        
        // Вызываем диалог
        GameLogic.Instance.DialogueSystem.StartDialogue(DialogueBase.Instance.GetDialogue(Id));
    }

    private void CompleteDialog1()
    {
        // Наносим боссу смертельный удар
        GameLogic.Instance.PumpkinBoss.GetComponent<HealthPoint>().Decrement(999999);
        
        // Меняем музыку локации
        SoundManager.Instance.SwitchBackground(0);
        
        // Завершаем диалог 2
        GameLogic.Instance.DialogueSystem.OnEnd += CompleteDialog2;
        
        // Вызываем диалог
        GameLogic.Instance.DialogueSystem.StartDialogue(DialogueBase.Instance.GetDialogue(QuestId.BossFightFinishCastEnd_OnlyDialog));
    }
    
    private void CompleteDialog2()
    {
        // Завершаем квест
        GameLogic.Instance.DialogueSystem.OnStart += () => { };
        GameLogic.Instance.DialogueSystem.OnEnd += CompleteQuest;
        
        // Вызываем диалог
        GameLogic.Instance.DialogueSystem.StartDialogue(DialogueBase.Instance.GetDialogue(QuestId.BringContinue_OnlyDialog));
    }
    
    protected override void DoAfterComplete()
    {
        // Деактивируем остановку бабули, если подошел персонаж
        GameLogic.Instance.WitchController.GetComponent<StopBehaviour>().Deactivate();
        
        // Запускаем игрока
        GameLogic.Instance.StartPlayer();
    }
}