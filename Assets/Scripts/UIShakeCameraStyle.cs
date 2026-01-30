using UnityEngine;
using Cysharp.Threading.Tasks;
using LitMotion;

public class UIShakeCameraStyle : MonoBehaviour
{
    RectTransform rect;
    Vector3 originalPos;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        originalPos = rect.localPosition;
    }

    public async UniTask Shake(float duration, float power)
    {
        await LMotion.Create(1f, 0f, duration)
                .WithEase(Ease.OutCubic)
                .Bind(t =>
                {
                    rect.localPosition =
                        originalPos + (Vector3)Random.insideUnitCircle * power * t;
                });

        rect.localPosition = originalPos;
    }
}