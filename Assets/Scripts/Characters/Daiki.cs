using UnityEngine;

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
    public override void SAActivate()
    {
        Debug.Log("Special Attack Activated for " + characterName);
        
    }

    public override void SADeactivate()
    {
        Debug.Log("Special Attack Deactivated for " + characterName);
    }
}