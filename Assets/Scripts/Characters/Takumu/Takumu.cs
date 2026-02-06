using UnityEngine;
using Cysharp.Threading.Tasks;

public class Takumu : Character
{
    int _takumuLayerIndex;

    [SerializeField] GameObject _housePrefab;
    [SerializeField] Vector2 _houseSpawnPosition;
    void Start()
    {
        Init(CharacterNames.Takumu);
        _takumuLayerIndex = Animator.GetLayerIndex("Takumu Layer");
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void SPActivate()
    {
        Debug.Log("Special Attack Activated for " + characterName);
        base.SPActivate();
        Animator.SetLayerWeight(_takumuLayerIndex, 1f);
        TakumuSP().Forget();
    }

    public override void SPDeactivate()
    {
        Debug.Log("Special Attack Deactivated for " + characterName);
        base.SPDeactivate();
        Animator.SetLayerWeight(_takumuLayerIndex, 0f);
    }

    async UniTask TakumuSP()
    {
        var dir = sr.flipX ? -1 : 1;
        var houseSpawnPosition = _houseSpawnPosition;
        houseSpawnPosition.x *= dir;
        await UniTask.Delay(700);
        var houseGo = Instantiate(_housePrefab, (Vector2)transform.position + houseSpawnPosition, Quaternion.identity);
        var house = houseGo.GetComponentInChildren<TakumuHouse>();
        house.Owner = this;
        await UniTask.Delay(1500);
        Destroy(houseGo);

        SPDeactivate();
    }
}