using UnityEngine;

public class SeiyuuCoconuts : HitBox
{
    Rigidbody2D _rb;
    [SerializeField] float launchSpeed = 20f;
    [SerializeField] GameObject _explosionPrefab;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        Damage = 12f;
        Knockback = new Vector2(8f, 2f);
        InvincibilityDuration = 0.1f;
        DownTime = 0.4f;
    }

    void Start()
    {
        var dir = Owner.sr.flipX ? -1 : 1;
        _rb.linearVelocity = new Vector2(dir * launchSpeed, 2f);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        var dir = Mathf.Sign(collision.transform.position.x - transform.position.x);
        Knockback.x *= dir;
        base.OnTriggerEnter2D(collision);
        Debug.Log("SeiyuuCoconuts hit " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("SeiyuuCoconuts exploded on " + collision.gameObject.name);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}