using UnityEngine;

public class ShioriExpress : HitBox
{
    Rigidbody2D _rb;
    [SerializeField] float Speed = 15f;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        Damage = 7f;
        Knockback = new Vector2(0f, 7f);
        InvincibilityDuration = 0.1f;
        DownTime = 0.5f;
    }

    public void Launch(int direction)
    {
        _rb.linearVelocity = new Vector2(Speed * direction, 0f);
    }
}