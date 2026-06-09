using System;
using UnityEngine;

public class DialogueWitchMoveAction: MonoBehaviour, IDialogueAction
{
    [SerializeField] private Transform _point;
    
    public EDialogueActor Actor() => EDialogueActor.Witch;
    
    public void Execute()
    {
        GameLogic.Instance.WitchController.GetComponent<StopBehaviour>().Deactivate();
        GameLogic.Instance.WitchController.GetComponent<WitchPatrolBehaviour>().SetTarget(_point);
        GameLogic.Instance.WitchController.GetComponent<WitchPatrolBehaviour>().OnTargetReached += Stop;
    }

    private void Stop()
    {
        GameLogic.Instance.WitchController.GetComponent<StopBehaviour>().Activate();
        GameLogic.Instance.WitchController.GetComponent<WitchPatrolBehaviour>().Deactivate();
        GameLogic.Instance.WitchController.GetComponent<WitchPatrolBehaviour>().OnTargetReached -= Stop;
    }
}
