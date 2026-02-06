using UnityEngine;
using Cysharp.Threading.Tasks;

public class Seiyuu : Character
{
    void Start()
    {
        Init(CharacterNames.Seiyuu);
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void SPActivate()
    {
        Debug.Log("Special Attack Activated for " + characterName);
        base.SPActivate();
        SeiyuuSP().Forget();
    }

    public override void SPDeactivate()
    {
        Debug.Log("Special Attack Deactivated for " + characterName);
        base.SPDeactivate();
    }

    async UniTask SeiyuuSP()
    {
        

        SPDeactivate();
    }
}