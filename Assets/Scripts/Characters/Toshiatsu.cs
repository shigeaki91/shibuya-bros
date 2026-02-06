using UnityEngine;
using Cysharp.Threading.Tasks;

public class Toshiatsu : Character
{
    void Start()
    {
        Init(CharacterNames.Toshiatsu);
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void SPActivate()
    {
        Debug.Log("Special Attack Activated for " + characterName);
        base.SPActivate();
        ToshiatsuSP().Forget();
    }

    public override void SPDeactivate()
    {
        Debug.Log("Special Attack Deactivated for " + characterName);
        base.SPDeactivate();
    }

    async UniTask ToshiatsuSP()
    {
        

        SPDeactivate();
    }
}