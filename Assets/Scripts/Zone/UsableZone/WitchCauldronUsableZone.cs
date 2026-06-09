using System;
using TMPro;
using UnityEngine;

public class WitchCauldronUsableZone : BaseUsableObjectZone
{
    private bool _canBeUsed = false;
    
    public override void StayDetect(ActorController actorController)
    {
        if (GameUI.Instance && _canBeUsed)
        {
            GameUI.Instance.ShowNoty("Нажмите \"E\", чтобы бросить ингредиенты в котел!");
        }
    }
    
    public void Activate()
    {
        _canBeUsed = true;
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