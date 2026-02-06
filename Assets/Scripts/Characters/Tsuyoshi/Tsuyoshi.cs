using UnityEngine;
using Cysharp.Threading.Tasks;

public class Tsuyoshi : Character
{
    int _tsuyoshiLayerIndex;    
    [SerializeField] GameObject _shutterPrefab;
    [SerializeField] Vector2 _shutterSpawnPosition;
    void Start()
    {
        Init(CharacterNames.Tsuyoshi);
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void SPActivate()
    {
        Debug.Log("Special Attack Activated for " + characterName);
        base.SPActivate();
        TsuyoshiSP().Forget();
    }

    public override void SPDeactivate()
    {
        Debug.Log("Special Attack Deactivated for " + characterName);
        base.SPDeactivate();
    }

    async UniTask TsuyoshiSP()
    {
        var dir = sr.flipX ? -1 : 1;
        var shutterSpawnPosition = _shutterSpawnPosition;
        shutterSpawnPosition.x *= dir;
        await UniTask.Delay(100);
        var shutterGo = Instantiate(_shutterPrefab, (Vector2)transform.position + shutterSpawnPosition, Quaternion.identity);
        var shutter = shutterGo.GetComponent<TsuyoshiShutter>();
        shutter.Owner = this;
        shutter.Direction = dir;
        await UniTask.Delay(1200);

        SPDeactivate();
    }
}