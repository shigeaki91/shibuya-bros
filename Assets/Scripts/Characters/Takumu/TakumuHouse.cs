using UnityEngine;
using LitMotion;

public class TakumuHouse : HitBox
{
    void Awake()
    {
        Damage = 10f;
        Knockback = new Vector2(0f, 8f);
        InvincibilityDuration = 0.1f;
        DownTime = 0.8f;

        var Parent = transform.parent;

        LMotion.Create(0.1f, 1.5f, 0.1f)
            .WithEase(Ease.OutQuad)
            .Bind((float val) =>
            {
                Parent.localScale = Vector2.one * val;
            });
    }

    void Start()
    {
        AudioManager.Instance.PlaySFX(SFXtypes.HouseBuild);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}