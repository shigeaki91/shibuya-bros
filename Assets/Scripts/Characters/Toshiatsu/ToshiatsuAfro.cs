using UnityEngine;

public class ToshiatsuAfro : HitBox
{
    Rigidbody2D _rb;
    [SerializeField] GameObject _explosionPrefab;
    [SerializeField] Vector2 _initialVelocity = new Vector2(5f, 3f);
    public float Direction;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        Damage = 3f;
        Knockback = new Vector2(2f, 2f);
        InvincibilityDuration = 0.1f;
        DownTime = 0.4f;
    }
    void Start()
    {
        _rb.linearVelocity = _initialVelocity;
        var dir = Owner.sr.flipX ? -1 : 1;
        _rb.linearVelocityX *= dir;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        var dir = Mathf.Sign(collision.transform.position.x - transform.position.x);
        Knockback.x *= dir;
        base.OnTriggerEnter2D(collision);
        if (!collision.gameObject.CompareTag("Ground")) return;
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}