using System;
using System.Collections;
using UnityEngine;

public class DeadSwampQuest: BaseQuest
{
    public override QuestId Id => QuestId.DeadSwamp;

    public override string Description =>
        "Воскрешение";
    
    protected override void DoAfterStart()
    {
        // Останавливаем игрока
        GameLogic.Instance.StopPlayer();
        
        // Возраждаем игрока
        GameLogic.Instance.PlayerController.Respawn();
        GameLogic.Instance.PlayerHealthPoint.Increment(GameLogic.Instance.PlayerHealthPoint.MaxValue);
        
        // Активируем остановку бабули, если подошел персонаж
        GameLogic.Instance.WitchController.GetComponent<StopBehaviour>().Activate();
        
        // Начинаем диалог с Бабулей
        GameLogic.Instance.DialogueSystem.OnStart += () => { };
        GameLogic.Instance.DialogueSystem.OnEnd += CompleteQuest;
        GameLogic.Instance.DialogueSystem.StartDialogue(DialogueBase.Instance.GetDialogue(Id));
    }
    
    protected override void DoAfterComplete()
    {
        if (GameLogic.Instance.QuestSystem.CurrentQuest.Id == QuestId.CollectPoisonBells)
        {
            GameLogic.Instance.TeleportToSwamp();
        }
        
        // Запускаем игрока
        GameLogic.Instance.StartPlayer();
        
        // Деактивируем остановку бабули, если подошел персонаж
        GameLogic.Instance.WitchController.GetComponent<StopBehaviour>().Deactivate();
    }
}