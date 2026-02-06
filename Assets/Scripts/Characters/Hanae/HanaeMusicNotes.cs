using UnityEngine;

public class HanaeMusicNotes : HitBox
{
    Rigidbody2D _rb;
    [SerializeField] float launchSpeed = 10f;
    [SerializeField] float _existenceTime = 3f;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        Damage = 6.2f;
        Knockback = new Vector2(0f, 5f);
        InvincibilityDuration = 0.1f;
        DownTime = 0.4f;
    }

    public void SetSpeed(Vector2 angle)
    {
        _rb.linearVelocity = angle * launchSpeed;
    }

    void Start()
    {
        Destroy(gameObject, _existenceTime);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        var dir = Mathf.Sign(collision.transform.position.x - transform.position.x);
        Knockback.x *= dir;
        base.OnTriggerEnter2D(collision);
        Debug.Log("HanaeMusicNotes hit " + collision.gameObject.name);
        if (collision.gameObject != Owner)
        {
            Destroy(gameObject);
        }
    }
}