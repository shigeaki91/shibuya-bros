using VContainer;
using VContainer.Unity;
using UnityEngine;

public class MatchLifeTimeScope : LifetimeScope
{
    [SerializeField] PlayerLifetimeScope _playerScopePrefab;
    int _playerCount = 2;
    int _id;
    public int Id => _id;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_playerScopePrefab);
        Debug.Log("MatchLifeTimeScope configured.");
    }

    protected override void Awake()
    {
        for (int i = 1; i <= _playerCount; i++)
        {
            _id = i;
            var scope = Instantiate(_playerScopePrefab, transform);
            Debug.Log(scope.name);
            scope.name = $"PlayerScope_{i}";
        }
        
        base.Awake();
    }
}