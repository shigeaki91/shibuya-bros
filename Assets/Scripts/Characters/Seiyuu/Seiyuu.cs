using UnityEngine;
using Cysharp.Threading.Tasks;

public class Seiyuu : Character
{

    [SerializeField] GameObject _CanonPrefab;
    [SerializeField] GameObject _coconutPrefab;
    [SerializeField] Vector2 _canonSpawnPosition;
    [SerializeField] Vector2 _coconutSpawnPosition;
    void Start()
    {
        Init(CharacterNames.Seiyuu);
        _specialLayerIndex = Animator.GetLayerIndex("Seiyuu Layer");
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void SPActivate()
    {
        Debug.Log("Special Attack Activated for " + characterName);
        base.SPActivate();
        SeiyuuSP().Forget();
    }

    public override void SPDeactivate()
    {
        Debug.Log("Special Attack Deactivated for " + characterName);
        base.SPDeactivate();
    }

    async UniTask SeiyuuSP()
    {
        var dir = sr.flipX ? -1 : 1;
        var canonSpawnPosition = _canonSpawnPosition;
        canonSpawnPosition.x *= dir;
        var coconutSpawnPosition = _coconutSpawnPosition;
        coconutSpawnPosition.x *= dir;
        var canonGo = Instantiate(_CanonPrefab, (Vector2)transform.position + canonSpawnPosition, Quaternion.identity);
        var canon = canonGo.GetComponent<SeiyuuCanon>();
        canon.Owner = this;
        await UniTask.Delay(800);
        var coconutGo = Instantiate(_coconutPrefab, (Vector2)transform.position + coconutSpawnPosition, Quaternion.identity);
        var coconut = coconutGo.GetComponent<SeiyuuCoconuts>();
        coconut.Owner = this;
        AudioManager.Instance.PlaySFX(SFXtypes.Hassya);
        await UniTask.Delay(1000);
        Destroy(canonGo);
        Destroy(coconutGo);
        SPDeactivate();
    }
}