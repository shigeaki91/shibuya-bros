using System.Threading;
using UnityEngine;
using LitMotion;
using LitMotion.Extensions;
using Cysharp.Threading.Tasks;
using Microsoft.Unity.VisualStudio.Editor;


public enum FadeState
{
    FadeIn,
    FadeOut
}

[RequireComponent(typeof(CanvasGroup))]
public class FadeController : MonoBehaviour
{
    CanvasGroup _group;
    [SerializeField] float _duration = 1f;
    [SerializeField] FadeState _fadeState;

    void Awake()
    {
        _group = GetComponent<CanvasGroup>();
    }

    public async UniTask FadeIn(CancellationToken ct = default)
    {
        enabled = true;
        transform.SetAsLastSibling();
        if (_fadeState != FadeState.FadeIn)
        {
            return;
        }
        _group.alpha = 0f;

        await LMotion.Create(0f, 1f, _duration)
                    .BindToAlpha(_group)
                    .ToUniTask(ct);

        _group.alpha = 1f;
    }

    public async UniTask FadeOut(CancellationToken ct = default)
    {
        enabled = true;
        transform.SetAsLastSibling();
        if (_fadeState != FadeState.FadeOut)
        {
            return;
        }
        _group.alpha = 1f;

        await LMotion.Create(1f, 0f, _duration)
                            .BindToAlpha(_group)
                            .ToUniTask(ct);

        _group.alpha = 0f;
    }
}