using UnityEngine;
using Cysharp.Threading.Tasks;

public class Shigeaki : Character
{
    void Start()
    {
        Init(CharacterNames.Shigeaki);
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void SPActivate()
    {
        Debug.Log("Special Attack Activated for " + characterName);
        base.SPActivate();
        ShigeakiSP().Forget();
    }

    public override void SPDeactivate()
    {
        Debug.Log("Special Attack Deactivated for " + characterName);
        base.SPDeactivate();
    }

    async UniTask ShigeakiSP()
    {
        

        SPDeactivate();
    }
}