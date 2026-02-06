using UnityEngine;

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
    }

    public override void SPDeactivate()
    {
        Debug.Log("Special Attack Deactivated for " + characterName);
        base.SPDeactivate();
    }
}