using UnityEngine;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public class Daiki : Character
{
    int _baseLayerIndex;
    int _daikiLayerIndex;
    void Start()
    {
        Init(CharacterNames.Daiki);
        _baseLayerIndex = Animator.GetLayerIndex("Base Layer");
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
        DaikiSP().Forget();
    }

    public override void SPDeactivate()
    {
        Debug.Log("Special Attack Deactivated for " + characterName);
        base.SPDeactivate();
    }

    async UniTask DaikiSP()
    {
        

        SPDeactivate();
    }
}