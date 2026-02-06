using UnityEngine;
using Cysharp.Threading.Tasks;

public class Shiori : Character
{
    void Start()
    {
        Init(CharacterNames.Shiori);
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void SPActivate()
    {
        Debug.Log("Special Attack Activated for " + characterName);
        base.SPActivate();
        ShioriSP().Forget();
    }

    public override void SPDeactivate()
    {
        Debug.Log("Special Attack Deactivated for " + characterName);
        base.SPDeactivate();
    }

    async UniTask ShioriSP()
    {
        

        SPDeactivate();
    }
}