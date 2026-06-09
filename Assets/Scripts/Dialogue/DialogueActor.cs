using UnityEngine;

public class DialogueActor : MonoBehaviour, IDialogueActor
{
    [SerializeField] private EDialogueActor _dialogueActor;
    [SerializeField] private Transform _dialoguePoint;
    [SerializeField] string _nameText;

    public EDialogueActor Actor() => _dialogueActor;
    public string Name() => _nameText;
    public Transform Point() => _dialoguePoint;
}
