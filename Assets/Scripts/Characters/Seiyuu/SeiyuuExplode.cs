using UnityEngine;
using LitMotion;

public class SeiyuuExplode : HitBox
{
    [SerializeField] Animator _animator;
    float _exsistDuration = 0.4f;
    void Awake()
    {
        Damage = 15f;
        Knockback = new Vector2(6f, 6f);
        InvincibilityDuration = 0.1f;
        DownTime = 1.0f;
    }

    void Start()
    {
        _animator.Play("Explosion");
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