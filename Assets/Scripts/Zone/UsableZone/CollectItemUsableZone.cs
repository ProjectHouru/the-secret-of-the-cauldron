using System;
using TMPro;
using UnityEngine;

public class CollectItemUsableZone : BaseUsableObjectZone
{
    [SerializeField] private CollectableItem _item;
    
    private AudioSource _audioSource;
    
    private bool _canBeUsed = true;
    
    public override void StayDetect(ActorController actorController)
    {
        if (GameUI.Instance && _canBeUsed)
        {
            GameUI.Instance.ShowNoty("Нажмите клавишу \"Е\" для взаимодействия");
        }
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
        // Sound
        SoundManager.Instance.PlaySound(SoundType.ItemCollect, gameObject);
        
        // Скрываем подсказку
        GameUI.Instance.HideNoty();
        
        // Добавляем в инвентарь пользователя
        GameLogic.Instance.PlayerInventory.AddItem(_item);
        
        base.Use();
        
        _canBeUsed = false;
        
        // Скрываем предмет на сцене
        _item.gameObject.SetActive(false);
    }

    public override bool CanBeUsed()
    {
        return _canBeUsed;
    }
}