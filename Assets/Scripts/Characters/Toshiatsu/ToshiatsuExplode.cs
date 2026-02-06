using UnityEngine;
using LitMotion;

public class ToshiatsuExplode : HitBox
{
    float _exsistDuration = 0.3f;
    void Awake()
    {
        Damage = 18f;
        Knockback = new Vector2(7f, 7f);
        InvincibilityDuration = 0.1f;
        DownTime = 1.2f;

        LMotion.Create(0.3f, 1f, 0.1f)
            .WithEase(Ease.OutQuad)
            .Bind((float val) =>
            {
                transform.localScale = Vector3.one * val;
            });
    }

    void Start()
    {
        AudioManager.Instance.PlaySFX(SFXtypes.Explosion);
        Destroy(gameObject, _exsistDuration);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        var dir = Mathf.Sign(collision.transform.position.x - transform.position.x);
        Knockback.x *= dir;
        base.OnTriggerEnter2D(collision);
    }
}