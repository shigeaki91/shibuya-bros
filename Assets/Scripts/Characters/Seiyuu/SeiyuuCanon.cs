using UnityEngine;
using UnityEngine.UIElements;

public class SeiyuuCanon : HitBox
{
    [SerializeField] float _exsistDuration = 0.1f;
    BoxCollider2D _bc;
    SpriteRenderer _sr;
    void Awake()
    {
        _bc = GetComponent<BoxCollider2D>();
        _sr = GetComponent<SpriteRenderer>();
        Damage = 5f;
        Knockback = new Vector2(6f, 1f);
        InvincibilityDuration = 0.1f;
        DownTime = 0.3f;
    }

    void Start()
    {
        _sr.flipX = !Owner.sr.flipX;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    void Update()
    {
        _exsistDuration -= Time.deltaTime;
        if (_exsistDuration <= 0f)
        {
            _bc.enabled = false;
        }
    }
}