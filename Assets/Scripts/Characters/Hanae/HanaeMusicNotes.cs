using UnityEngine;

public class HanaeMusicNotes : HitBox
{
    Rigidbody2D _rb;
    [SerializeField] float launchSpeed = 5f;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        Damage = 8f;
        Knockback = new Vector2(0f, 5f);
        InvincibilityDuration = 0.1f;
        DownTime = 0.4f;
    }
}