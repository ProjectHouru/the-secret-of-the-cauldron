using UnityEngine;

public class AIController : MonoBehaviour
{
    private IEnemyBehaviour[] _actions;
    private ActorController _actorController;

    private void Start()
    {
        _actions = GetComponents<IEnemyBehaviour>();
        _actorController = GetComponent<ActorController>();
    }

    private void Update()
    {
        if (!_actorController.IsDead)
        {
            foreach (var action in _actions)
            {
                action?.Execute();
            }
        }
    }
}