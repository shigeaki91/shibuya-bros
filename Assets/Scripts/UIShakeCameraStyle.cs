using UnityEngine;
using Cysharp.Threading.Tasks;
using LitMotion;

public class UIShakeCameraStyle : MonoBehaviour
{
    [SerializeField] RectTransform rect;
    Vector3 originalPos;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        originalPos = rect.localPosition;
    }

    public async UniTask Shake(float duration, float power)
    {
        Debug.Log($"timeScale: {Time.timeScale}");
        await LMotion.Create(1f, 0f, duration)
                .WithEase(Ease.OutCubic)
                .Bind(t =>
                {
                    rect.localPosition = originalPos + (Vector3)Random.insideUnitCircle * power * t;
                })
                .ToUniTask(this.GetCancellationTokenOnDestroy());
        Debug.Log("Shake Complete");

        rect.localPosition = originalPos;
    }
}