using System;
using TMPro;
using UnityEngine;

public class WitchUsableZone : BaseUsableObjectZone
{
    private bool _canBeUsed = false;
    
    public override void StayDetect(ActorController actorController)
    {
        if (GameUI.Instance && _canBeUsed)
        {
            GameUI.Instance.ShowNoty("Нажмите \"E\", чтобы поговорить с бабулькой!");
        }
    }
    
    public void Activate()
    {
        _canBeUsed = true;
    }

    public void Deactivate()
    {
        _canBeUsed = false;
    }


    public override void Miss()
    {
        if (GameUI.Instance && _canBeUsed)
        {
            GameUI.Instance.HideNoty();
        }
    }

    public override void Use()
    {
        if (GameLogic.Instance)
        {
            GameLogic.Instance.QuestSystem.CurrentQuest.TriggerQuest();
        }
        
        _canBeUsed = false;
        
        // Скрываем подсказку
        GameUI.Instance.HideNoty();
        
        base.Use();
    }

    public override bool CanBeUsed()
    {
        return _canBeUsed;
    }
}