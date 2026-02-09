using UnityEngine;

class PrivacyManager : MonoBehaviour
{
    public static PrivacyManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            // RootLifetimeScopeで生成するため、DontDestroyOnLoadは不要
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}