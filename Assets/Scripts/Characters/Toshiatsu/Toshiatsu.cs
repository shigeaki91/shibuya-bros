using UnityEngine;
using Cysharp.Threading.Tasks;

public class Toshiatsu : Character
{
    int _toshiatsuLayerIndex;
    [SerializeField] GameObject _afroPrefab;
    [SerializeField] Vector2 _afroSpawnPosition;
    void Start()
    {
        Init(CharacterNames.Toshiatsu);
        _toshiatsuLayerIndex = Animator.GetLayerIndex("Toshiatsu Layer");
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void SPActivate()
    {
        Debug.Log("Special Attack Activated for " + characterName);
        base.SPActivate();
        Animator.SetLayerWeight(_toshiatsuLayerIndex, 1f);
        ToshiatsuSP().Forget();
    }

    public override void SPDeactivate()
    {
        Debug.Log("Special Attack Deactivated for " + characterName);
        base.SPDeactivate();
        Animator.SetLayerWeight(_toshiatsuLayerIndex, 0f);
    }

    async UniTask ToshiatsuSP()
    {
        //Animator.SetTrigger("ToshiatsuSP");
        var dir = sr.flipX ? -1 : 1;
        var afroSpawnPosition = _afroSpawnPosition; 
        afroSpawnPosition.x *= dir;
        await UniTask.Delay(200);
        var afroGo = Instantiate(_afroPrefab, (Vector2)transform.position + afroSpawnPosition, Quaternion.identity);
        var afro = afroGo.GetComponent<ToshiatsuAfro>();
        afro.Owner = this;
        await UniTask.Delay(1500);


        SPDeactivate();
    }
}