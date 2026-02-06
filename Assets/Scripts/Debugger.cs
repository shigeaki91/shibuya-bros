using UnityEngine;

public class Debugger : MonoBehaviour
{
    void OnDisable()
    {
        Debug.Log($"[DISABLED] {name}", this);
    }
}
