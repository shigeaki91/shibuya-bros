using UnityEngine;
using LitMotion;
using Cysharp.Threading.Tasks;
using LitMotion.Extensions;

public class TsuyoshiShutter : HitBox
{
    [SerializeField] Vector2 _targetPos1;
    [SerializeField] Vector2 _targetPos2;
    [SerializeField] Vector2 _targetPos3;
    BoxCollider2D _bc;
    SpriteRenderer _sr;
    void Awake()
    {
        _bc = GetComponent<BoxCollider2D>();
        _sr = GetComponent<SpriteRenderer>();
        Damage = 6f;
        Knockback = new Vector2(0f, 0f);
        InvincibilityDuration = 0.1f;
        DownTime = 1.5f;
        _bc.enabled = false;
        _sr.color = new Color(_sr.color.r, _sr.color.g, _sr.color.b, 0.4f);
    }

    async void Start()
    {
        await LMotion.Create((Vector2)transform.position, (Vector2)transform.position + _targetPos1, 0.3f)
                .WithEase(Ease.OutQuad)
                .BindToPositionXY(transform)
                .ToUniTask();

        await LMotion.Create((Vector2)transform.position, (Vector2)transform.position + _targetPos2, 0.3f)
                .BindToPositionXY(transform)
                .ToUniTask();

        await LMotion.Create((Vector2)transform.position, (Vector2)transform.position + _targetPos3, 0.1f)
                .WithEase(Ease.InQuad)
                .BindToPositionXY(transform)
                .ToUniTask();

        await UniTask.Delay(200);
        _bc.enabled = true;
        await LMotion.Create(0.4f, 1f, 0.1f)
                .WithEase(Ease.OutQuad)
                .Bind((float val) =>
                {
                    _sr.color = new Color(_sr.color.r, _sr.color.g, _sr.color.b, val);
                })
                .ToUniTask();

        _bc.enabled = false;
        await LMotion.Create(1f, 0f, 0.1f)
                .WithEase(Ease.InQuad)
                .Bind((float val) =>
                {
                    _sr.color = new Color(_sr.color.r, _sr.color.g, _sr.color.b, val);
                })
                .ToUniTask();
        
        Destroy(gameObject);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

}