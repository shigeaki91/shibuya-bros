using UnityEngine;
using Cysharp.Threading.Tasks;

public class Hanae : Character
{
    void Start()
    {
        Init(CharacterNames.Hanae);
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void SPActivate()
    {
        Debug.Log("Special Attack Activated for " + characterName);
        base.SPActivate();
        HanaeSP().Forget();
    }

    public override void SPDeactivate()
    {
        Debug.Log("Special Attack Deactivated for " + characterName);
        base.SPDeactivate();
    }

    async UniTask HanaeSP()
    {
        

        SPDeactivate();
    }
}