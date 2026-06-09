using System;
using UnityEngine;

public class BringPumpkinsToCauldron: BaseQuest
{
    [SerializeField] private Transform _pumpkinBossTeleportPoint;
    [SerializeField] private GameObject _fightZoneBorder;
    public override QuestId Id => QuestId.BringPumpkinsToCauldron;

    public override string Description =>
        "Брось собранные тыквы в котел";
      

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
        GameLogic.Instance.PlayerInventory.RemoveItem(ItemType.Teeth, GameLogic.Instance.PlayerInventory.GetItemQuantity(ItemType.Teeth));

        // Загружаем босса
        PrepareBossZone();
        
        // Останавливаем игрока
        GameLogic.Instance.DialogueSystem.OnStart += GameLogic.Instance.StopPlayer;
        
        // Начинаем диалог с Бабулей
        GameLogic.Instance.DialogueSystem.OnEnd += CompleteQuest;
        GameLogic.Instance.DialogueSystem.StartDialogue(DialogueBase.Instance.GetDialogue(Id));
    }

    private void PrepareBossZone()
    {
        // Активация (если запускаем квест вручную)
        if (!GameLogic.Instance.PumpkinBoss.gameObject.activeSelf)
        {
            GameLogic.Instance.PumpkinBoss.gameObject.SetActive(true);
        }
        
        // Останавливаем босса и телепортируем в зону Бабули
        GameLogic.Instance.PumpkinBoss.gameObject.transform.position = _pumpkinBossTeleportPoint.position;
        GameLogic.Instance.PumpkinBoss.Stop();
        
        // Закрываем локацию
        _fightZoneBorder.SetActive(true);
        
        // Запускаем музыку
        GameLogic.Instance.PlayBossMusic();
    }
    
    protected override void DoAfterComplete()
    {
        // Запускаем босса
        GameLogic.Instance.PumpkinBoss.Run();
        
        // Запускаем игрока
        GameLogic.Instance.StartPlayer();
        
        // Деактивируем остановку бабули, если подошел персонаж
        GameLogic.Instance.WitchController.GetComponent<StopBehaviour>().Deactivate();
    }
}