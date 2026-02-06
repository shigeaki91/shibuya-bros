using UnityEngine;
using Cysharp.Threading.Tasks;
using R3.Triggers;

public class Daiki : Character
{
    int _daikiLayerIndex;

    [SerializeField] GameObject _ShadowPrefab;
    [SerializeField] GameObject _puddingPrefab;
    [SerializeField] Vector2 _shadowSpawnPosition;
    [SerializeField] Vector2 _puddingSpawnPosition;
    void Start()
    {
        Init(CharacterNames.Daiki);
        _daikiLayerIndex = Animator.GetLayerIndex("Daiki Layer");
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void SPActivate()
    {
        Debug.Log("Special Attack Activated for " + characterName);
        base.SPActivate();
        Animator.SetLayerWeight(_daikiLayerIndex, 1f);
        DaikiSP().Forget();
    }

    public override void SPDeactivate()
    {
        Debug.Log("Special Attack Deactivated for " + characterName);
        base.SPDeactivate();
        Animator.SetLayerWeight(_daikiLayerIndex, 0f);
    }

    async UniTask DaikiSP()
    {
        var dir = sr.flipX ? -1 : 1;
        var shadowSpawnPosition = _shadowSpawnPosition;
        shadowSpawnPosition.x *= dir;
        var puddingSpawnPosition = _puddingSpawnPosition;
        puddingSpawnPosition.x *= dir;
        await UniTask.Delay(300);
        var shadowGo = Instantiate(_ShadowPrefab, (Vector2)transform.position + shadowSpawnPosition, Quaternion.identity);
        var puddingGo = Instantiate(_puddingPrefab, (Vector2)transform.position + puddingSpawnPosition, Quaternion.identity);
        var pudding = puddingGo.GetComponent<DaikiPudding>();
        pudding.Owner = this;
        await UniTask.Delay((int)((_puddingSpawnPosition.y+1.5f) / -pudding.FallSpeed * 1000));
        await UniTask.Delay(500);
        Destroy(shadowGo);
        Destroy(puddingGo);
        SPDeactivate();
    }
}