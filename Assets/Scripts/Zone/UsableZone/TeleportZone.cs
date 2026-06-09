using UnityEngine;

public class TeleportZone : BaseUsableObjectZone
{
    [SerializeField] private Transform _toPoint;
    [SerializeField] private string _pointText = " в другую зону!";
    [SerializeField] private bool _canBeUsed = false;
    [SerializeField] private int _backgroundIndex = 0;
    
    public void Activate()
    {
        _canBeUsed = true;
    }
    
    public override void StayDetect(ActorController actorController)
    {
        if (GameUI.Instance && _canBeUsed)
        {
            GameUI.Instance.ShowNoty("Нажмите E чтобы отправится " + _pointText);
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
        SoundManager.Instance.PlaySound(SoundType.Teleport);
        SoundManager.Instance.SwitchBackground(_backgroundIndex);
        
        // Скрываем подсказку
        GameUI.Instance.HideNoty();

        GameLogic.Instance.PlayerController.gameObject.transform.position = _toPoint.position;
        
        base.Use();
        
        _canBeUsed = false;
    }
    
    public override bool CanBeUsed()
    {
        return _canBeUsed;
    }
}