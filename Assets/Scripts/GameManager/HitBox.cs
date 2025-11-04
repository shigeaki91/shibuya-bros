using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Character owner;
    public float damage = 10f;
    public Vector2 knockback = new Vector2(2f, 5f);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character target = collision.GetComponent<Character>();
        if (target != null && target != owner)
        {
            target.TakeDamage(damage);
            target.GetComponent<Rigidbody2D>().AddForce(knockback, ForceMode2D.Impulse);
        }

    }
}