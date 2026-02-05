using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

[System.Serializable]
struct CharaPrefabEntry
{
    public CharacterNames CharacterName;
    public GameObject CharacterPrefab;
}
public class GameManager : MonoBehaviour
{
    public Sprite[] selectedCharacterSprites = new Sprite[2];
    public CharacterNames[] selectedCharacters = new CharacterNames[2];
    public GameObject[] selectedCharacterPrefabs = new GameObject[2];
    public static GameManager Instance { get; private set; }
    [SerializeField] CharaPrefabEntry[] _charaPrefabEntries;
    Dictionary<CharacterNames, GameObject> _charaPrefabDict;
    [SerializeField] GameObject _matchScopePrefab;

    void Awake()
    {
        if (Instance == null)
        {
            // RootLifetimeScopeで生成するため、DontDestroyOnLoadは不要
            Instance = this;
            foreach (var entry in _charaPrefabEntries)
            {
                _charaPrefabDict ??= new Dictionary<CharacterNames, GameObject>();
                _charaPrefabDict[entry.CharacterName] = entry.CharacterPrefab;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCharacters(Sprite charaSprite, CharacterNames character, int index)
    {
        selectedCharacterSprites[index] = charaSprite;
        selectedCharacters[index] = character;
        selectedCharacterPrefabs[index] = _charaPrefabDict[character];
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "CharaSelect":
                OnEnterSelectScene(scene);
                break;

            case "BeforeMatch":
                OnEnterBeforeMatchScene(scene);
                break;

            case "Match":
                OnEnterMatchScene(scene);
                break;
        }
    }

    void OnEnterSelectScene(Scene scene)
    {
        selectedCharacterSprites = new Sprite[2];
        selectedCharacters = new CharacterNames[2];
        selectedCharacterPrefabs = new GameObject[2];
    }

    void OnEnterBeforeMatchScene(Scene scene)
    {
        // 試合前シーンに入ったときの処理
    }

    async void OnEnterMatchScene(Scene scene)
    {
        var matchScope = Instantiate(_matchScopePrefab);
        SceneManager.MoveGameObjectToScene(matchScope, scene);
        await Task.Delay(3500);
        var characters = FindObjectsByType<Character>(FindObjectsSortMode.None);
        foreach (var chara in characters)
        {
            chara.isStarting = false;
        }
    }
}