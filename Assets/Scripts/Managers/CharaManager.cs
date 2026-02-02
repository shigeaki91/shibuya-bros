using UnityEngine;

public class CharaManager : MonoBehaviour
{
    public CharacterNames[] selectedCharacters = new CharacterNames[2];
    public static CharaManager Instance { get; private set; }

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