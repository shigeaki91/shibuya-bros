using UnityEngine;

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
    public override void SAActivate()
    {
        Debug.Log("Special Attack Activated for " + characterName);
        
    }

    public override void SADeactivate()
    {
        Debug.Log("Special Attack Deactivated for " + characterName);
    }
}