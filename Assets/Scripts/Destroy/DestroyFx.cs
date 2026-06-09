using UnityEngine;

public class DestroyFx : MonoBehaviour
{
    public void OnAnimationCompleteHandler()
    {
        Destroy(gameObject);
    }
}