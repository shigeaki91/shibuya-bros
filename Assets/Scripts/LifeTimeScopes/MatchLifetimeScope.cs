using VContainer;
using VContainer.Unity;
using UnityEngine;
using System.Threading.Tasks;

public class MatchLifeTimeScope : LifetimeScope
{
    [SerializeField] PlayerLifetimeScope _playerScopePrefab;
    [SerializeField] CharacterNames[] _selectedCharacters = new CharacterNames[2];
    [SerializeField] GameObject[] _characterPrefabs = new GameObject[2];
    public CharacterNames[] SelectedCharacters => _selectedCharacters;
    public GameObject[] CharacterPrefabs => _characterPrefabs;
    int _playerCount = 2;
    int _id;
    public int Id => _id;
    Vector3 _spawnPoint;
    public Vector3 SpawnPoint => _spawnPoint;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_playerScopePrefab);
        Debug.Log("MatchLifeTimeScope configured.");
    }

    async protected override void Awake()
    {
        base.Awake();
        var matchManager = FindAnyObjectByType<MatchManager>();
        var groupTargetSetting = FindAnyObjectByType<GroupTargetSetting>();
        matchManager.gameObject.SetActive(false);
        groupTargetSetting.gameObject.SetActive(false);
        await Task.Delay(1);
        for (int i = 1; i <= _playerCount; i++)
        {
            _id = i;
            _selectedCharacters[i - 1] = GameManager.Instance.selectedCharacters[i - 1];
            _characterPrefabs[i - 1] = GameManager.Instance.selectedCharacterPrefabs[i - 1];
            _spawnPoint = new Vector3(6f * (i*2 - 3), 1.834587f, 0f);
            var scope = Instantiate(_playerScopePrefab, transform);
            Debug.Log(scope.name);
            scope.name = $"PlayerScope_{i}";
            await Task.Delay(1000);
        }

        matchManager.gameObject.SetActive(true);
        groupTargetSetting.gameObject.SetActive(true);
    }
}