using UnityEngine;

public class Shigeaki : Character
{
    void Start()
    {
        Init("Shigeaki");
    }
    protected override void Update()
    {
        base.Update();
        HandleInput();
    }
}
