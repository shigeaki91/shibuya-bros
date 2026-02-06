using UnityEngine;
using LitMotion;

public class SeiyuuExplode : HitBox
{
    float _exsistDuration = 0.3f;
    void Awake()
    {
        Damage = 15f;
        Knockback = new Vector2(6f, 6f);
        InvincibilityDuration = 0.1f;
        DownTime = 1.0f;

        LMotion.Create(0.3f, 1f, 0.1f)
            .WithEase(Ease.OutQuad)
            .Bind((float val) =>
            {
                transform.localScale = Vector3.one * val;
            });
    }

    void Update()
    {
        _exsistDuration -= Time.deltaTime;
        if (_exsistDuration <= 0f)
        {
            Destroy(gameObject);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        var dir = Mathf.Sign(collision.transform.position.x - transform.position.x);
        Knockback.x *= dir;
        base.OnTriggerEnter2D(collision);
    }
}