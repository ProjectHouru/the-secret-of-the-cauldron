using System;
using System.Collections;
using UnityEngine;

public class DeadWitchPumpkin: BaseQuest
{
    public override QuestId Id => QuestId.DeadWitchPumpkin;

    public override string Description =>
        "Воскрешение";
    
    protected override void DoAfterStart()
    {
        // Останавливаем тыкву
        GameLogic.Instance.PumpkinBoss.Stop();
        
        // Останавливаем игрока
        GameLogic.Instance.StopPlayer();
        
        // Возраждаем игрока
        GameLogic.Instance.PlayerController.Respawn();
        GameLogic.Instance.PlayerHealthPoint.Increment(GameLogic.Instance.PlayerHealthPoint.MaxValue);
        
        // Деактивируем зону действия бабули
        GameLogic.Instance.WitchController.GetComponent<WitchUsableZone>().Deactivate();
        
        // Активируем остановку бабули, если подошел персонаж
        GameLogic.Instance.WitchController.GetComponent<StopBehaviour>().Activate();
        
        // Начинаем диалог с Бабулей
        GameLogic.Instance.DialogueSystem.OnStart += () => { };
        GameLogic.Instance.DialogueSystem.OnEnd += CompleteQuest;
        GameLogic.Instance.DialogueSystem.StartDialogue(DialogueBase.Instance.GetDialogue(Id));
    }
    
    protected override void DoAfterComplete()
    {
        // Запускаем игрока
        GameLogic.Instance.StartPlayer();
        
        // Запускаем тыкву через 2 секунды
        StartCoroutine(StartPumpkin());

        // Перезапускаем квест битвы с боссом
        GameLogic.Instance.QuestSystem.SetCurrentQuest(QuestId.BossFight);
        
        // Деактивируем остановку бабули, если подошел персонаж
        GameLogic.Instance.WitchController.GetComponent<StopBehaviour>().Deactivate();
    }

    private IEnumerator StartPumpkin()
    {
        yield return new WaitForSeconds(2f);
        
        GameLogic.Instance.PumpkinBoss.Run();
    }
}