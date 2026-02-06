using UnityEngine;
using Cysharp.Threading.Tasks;

public class Takumu : Character
{
    void Start()
    {
        Init(CharacterNames.Takumu);
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void SPActivate()
    {
        Debug.Log("Special Attack Activated for " + characterName);
        base.SPActivate();
        TakumuSP().Forget();
    }

    public override void SPDeactivate()
    {
        Debug.Log("Special Attack Deactivated for " + characterName);
        base.SPDeactivate();
    }

    async UniTask TakumuSP()
    {
        

        SPDeactivate();
    }
}