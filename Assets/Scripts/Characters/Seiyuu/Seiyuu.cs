using UnityEngine;
using Cysharp.Threading.Tasks;

public class Seiyuu : Character
{
    int _seiyuuLayerIndex;

    [SerializeField] GameObject _CanonPrefab;
    [SerializeField] GameObject _coconutPrefab;
    [SerializeField] Vector2 _canonSpawnPosition;
    [SerializeField] Vector2 _coconutSpawnPosition;
    void Start()
    {
        Init(CharacterNames.Seiyuu);
        _seiyuuLayerIndex = Animator.GetLayerIndex("Seiyuu Layer");
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void SPActivate()
    {
        Debug.Log("Special Attack Activated for " + characterName);
        base.SPActivate();
        Animator.SetLayerWeight(_seiyuuLayerIndex, 1f);
        SeiyuuSP().Forget();
    }

    public override void SPDeactivate()
    {
        Debug.Log("Special Attack Deactivated for " + characterName);
        base.SPDeactivate();
        Animator.SetLayerWeight(_seiyuuLayerIndex, 0f);
    }

    async UniTask SeiyuuSP()
    {
        //Animator.SetTrigger("SeiyuuSP");
        var dir = sr.flipX ? -1 : 1;
        var canonSpawnPosition = _canonSpawnPosition;
        canonSpawnPosition.x *= dir;
        var coconutSpawnPosition = _coconutSpawnPosition;
        coconutSpawnPosition.x *= dir;
        await UniTask.Delay(700);
        var canonGo = Instantiate(_CanonPrefab, (Vector2)transform.position + canonSpawnPosition, Quaternion.identity);
        var canon = canonGo.GetComponent<SeiyuuCanon>();
        canon.Owner = this;
        await UniTask.Delay(500);
        var coconutGo = Instantiate(_coconutPrefab, (Vector2)transform.position + coconutSpawnPosition, Quaternion.identity);
        var coconut = coconutGo.GetComponent<SeiyuuCoconuts>();
        coconut.Owner = this;
        await UniTask.Delay(1500);
        Destroy(canonGo);
        Destroy(coconutGo);
        SPDeactivate();
    }
}