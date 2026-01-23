using VContainer;
using VContainer.Unity;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Extensions;
using UnityEngine.InputSystem;

public class PlayerLifetimeScope : LifetimeScope
{
    [SerializeField] MatchLifeTimeScope _matchScope;
    public int _playerID;
    Vector3 _spawnPoint;
    [SerializeField] GameObject _playerPrefab;
    public Character _player;
    public Dictionary<AttackTypes, HitBox> _hitBoxes;
    List<HitBox> _weakAttackHitBoxes;
    [SerializeField] InputActionAsset _inputActions;
    InputActionMap _inputActionMap;
    [Header("Configs")]
    [SerializeField] AirDownConfig _airDownConfig;
    [SerializeField] AirNeutralConfig _airNeutralConfig;
    [SerializeField] AirSideConfig _airSideConfig;
    [SerializeField] AirUpConfig _airUpConfig;
    [SerializeField] DashAttackConfig _dashAttackConfig;
    [SerializeField] SideSmashConfig _sideSmashConfig;
    [SerializeField] UpSmashConfig _upSmashConfig;
    [SerializeField] WeakAttackConfig _weakAttackConfig;
    protected override void Configure(IContainerBuilder builder)
    {
        Debug.Log($"Player {_playerID} dependencies configured.");
        
        foreach (var pair in _hitBoxes)
        {
            builder.RegisterInstance(pair.Value).Keyed(pair.Key);
        }
        builder.RegisterInstance(_weakAttackHitBoxes).Keyed(AttackTypes.WeakAttack);

        builder.RegisterInstance(ObservableEx.InputActionAsObservable(_inputActionMap.FindAction("Attack")));
        builder.RegisterInstance(_player);

        builder.RegisterInstance(_airDownConfig);
        builder.RegisterInstance(_airNeutralConfig);
        builder.RegisterInstance(_airSideConfig);
        builder.RegisterInstance(_airUpConfig);
        builder.RegisterInstance(_dashAttackConfig);
        builder.RegisterInstance(_sideSmashConfig);
        builder.RegisterInstance(_upSmashConfig);
        builder.RegisterInstance(_weakAttackConfig);

        builder.Register<AirDown>(Lifetime.Scoped);
        builder.Register<AirNeutral>(Lifetime.Scoped);
        builder.Register<AirSide>(Lifetime.Scoped);
        builder.Register<AirUp>(Lifetime.Scoped);
        builder.Register<DashAttack>(Lifetime.Scoped);
        builder.Register<SideSmash>(Lifetime.Scoped);
        builder.Register<UpSmash>(Lifetime.Scoped);
        builder.Register<WeakAttack>(Lifetime.Scoped);

        builder.RegisterBuildCallback(resolver =>
        {
            resolver.Resolve<AirDown>();
            resolver.Resolve<AirNeutral>();
            resolver.Resolve<AirSide>();
            resolver.Resolve<AirUp>();
            resolver.Resolve<DashAttack>();
            resolver.Resolve<SideSmash>();
            resolver.Resolve<UpSmash>();
            resolver.Resolve<WeakAttack>();
        });

        Debug.Log($"Player {_playerID} dependencies configured.");
    }

    protected override void Awake()
    {
        _matchScope = GetComponentInParent<MatchLifeTimeScope>();
        _playerID = _matchScope.Id;
        _spawnPoint = _matchScope.SpawnPoint;
        var playerGo = Instantiate(_playerPrefab, _spawnPoint, Quaternion.identity, transform);
        _player = playerGo.GetComponent<Character>();
        _player.PlayerID = _playerID;
        _player.name = $"Player_{_playerID}";
        _inputActionMap = _inputActions.FindActionMap($"Player{_playerID}");
        _inputActionMap.Enable();

        var hitBoxes = playerGo.GetComponentsInChildren<HitBox>();
        _hitBoxes = hitBoxes.ToDictionary(hb => hb.AttackType, hb => hb);
        
        _weakAttackHitBoxes = hitBoxes.Where(hb => hb.AttackType == AttackTypes.WeakAttack0
        || hb.AttackType == AttackTypes.WeakAttack1
        || hb.AttackType == AttackTypes.WeakAttack2).ToList();
        base.Awake();
    }

    public void SetPlayerID(int id)
    {
        _playerID = id;
    }
}
