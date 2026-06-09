using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingMovementQuest : BaseQuest
{
    [SerializeField] private Transform _startPoint;
    [SerializeField] private PlayerDetectZone _targetZone;

    public override QuestId Id => QuestId.OnboardingMovement;

    public override string Description =>
        "Обучение (перемещение)\n" +
        "Идите дальше по тропинке";

    protected override void DoAfterStart()
    {
        GameLogic.Instance.PlayerController.StopFire = true;
        GameUI.Instance.ShowNoty("Для управления персонажем используйте клавиши W, A, S, D на клавиатуре", 5f);

        // Перемещаем игрока в зону старта
        GameLogic.Instance.PlayerController.gameObject.transform.position = _startPoint.position;
        
        _targetZone.OnPlayerEnter += OnTriggerEndZoneEnter;
        
        if (_targetZone != null)
        {
            _targetZone.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEndZoneEnter()
    {
        GameUI.Instance.HideNoty();
        
        CompleteQuest();
        
        Debug.Log("Квест завершён");
    }

    protected override void DoAfterComplete()
    {
        if (_targetZone != null)
        {
            _targetZone.gameObject.SetActive(false);
        }
    }
}