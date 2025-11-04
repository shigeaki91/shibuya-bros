using UnityEngine;

public class Stage : MonoBehaviour
{
    public string stageName;
    public Vector2 stageSize;
    public Vector2 deathZone = new Vector2(50f, 30f);

    public bool IsOutOfBounds(Transform characterTransform)
    {
        return Mathf.Abs(characterTransform.position.x) > deathZone.x || Mathf.Abs(characterTransform.position.y) > deathZone.y;
    }
}
