using UnityEngine;
using Cysharp.Threading.Tasks;

public class Tsuyoshi : Character
{
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
        

        SPDeactivate();
    }
}