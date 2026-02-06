using UnityEngine;

public class DaikiPudding : HitBox
{
    Rigidbody2D _rb;
    public float FallSpeed = -5f;
    bool hasLanded = false;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        Damage = 15f;
        Knockback = new Vector2(0f, -7f);
        InvincibilityDuration = 0.1f;
        DownTime = 0.5f;

        _rb.linearVelocityY = FallSpeed;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (!hasLanded)
            {
                hasLanded = true;
                _rb.linearVelocity = Vector2.zero;
                _rb.bodyType = RigidbodyType2D.Static;
                AudioManager.Instance.PlaySFX(SFXtypes.PuddingLand);
            }
        }
    }
}