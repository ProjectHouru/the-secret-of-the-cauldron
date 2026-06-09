public class DialogueMessage
{
    public EDialogueActor Actor;
    public string Message;
    public IDialogueAction StartAction;
    public IDialogueAction EndAction;

    public DialogueMessage(EDialogueActor actor, string message, IDialogueAction dialogueStartAction = null,
        IDialogueAction dialogueEndAction = null)
    {
        Actor = actor;
        Message = message;
        StartAction = dialogueStartAction;
        EndAction = dialogueEndAction;
    }
}
