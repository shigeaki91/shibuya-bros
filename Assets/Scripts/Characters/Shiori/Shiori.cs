using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

public class Shiori : Character
{
    int _shioriLayerIndex;

    [SerializeField] GameObject _expressPrefab;
    [SerializeField] Vector2 _expressSpawnPosition;
    void Start()
    {
        Init(CharacterNames.Shiori);
        _shioriLayerIndex = Animator.GetLayerIndex("Shiori Layer");
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void SPActivate()
    {
        Debug.Log("Special Attack Activated for " + characterName);
        base.SPActivate();
        Animator.SetLayerWeight(_shioriLayerIndex, 1f);
        ShioriSP().Forget();
    }

    public override void SPDeactivate()
    {
        Debug.Log("Special Attack Deactivated for " + characterName);
        base.SPDeactivate();
        Animator.SetLayerWeight(_shioriLayerIndex, 0f);
    }

    async UniTask ShioriSP()
    {
        var dir = Random.value < 0.5f ? -1 : 1;
        var expressSpawnPosition = _expressSpawnPosition;
        expressSpawnPosition.x *= dir;
        await UniTask.Delay(500);
        var expressGo = Instantiate(_expressPrefab, transform.position, Quaternion.identity);
        expressGo.transform.SetParent(null);
        expressGo.transform.position = expressSpawnPosition;
        var express = expressGo.GetComponent<ShioriExpress>();
        express.Owner = this;
        express.Launch(-dir);
        await UniTask.Delay(1000);
        Destroy(expressGo);

        SPDeactivate();
    }
}