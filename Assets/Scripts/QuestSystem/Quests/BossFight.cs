using System;
using UnityEngine;

public class BossFight: BaseQuest
{
    [SerializeField] private float _bossTimer = 10f;
    
    public override QuestId Id => QuestId.BossFight;

    private float _timer = 0f;
    private bool _isStartTimer = false;
    
    public override string Description =>
        "Выживете <color=red>" + Mathf.Round(_timer) + " из " + Mathf.Round(_bossTimer) + "</color> секунд";

    private void Update()
    {
        if (_isStarted && !_isCompleted && !_isPaused && _isStartTimer)
        {
            _timer += Time.deltaTime;
            
            UpdateQuest();

            if (_timer >= _bossTimer)
            {
                // Останавливаем таймер
                _isStartTimer = false;
                
                // Завершаем квест
                CompleteQuest();
            }
        }
    }

    protected override void DoAfterStart()
    {
        _timer = 0;
        _isStartTimer = true;
        
        GameUI.Instance.ShowNoty("Бить бесполезно! Бегите!");
    }
    
    protected override void DoAfterComplete()
    {
        GameUI.Instance.HideNoty();
    }
}