using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    public string attackName;
    public float damage;
    public Vector2 knockback;
    public float duration;
    protected bool isActive;
    protected float timer;

    protected Character owner;

    public virtual void Init(Character owner)
    {
        this.owner = owner;
        isActive = false;
        timer = 0f;
    }

    public virtual void Activate()
    {
        isActive = true;
        owner.canMove = false;
        timer = duration;
        Debug.Log($"{attackName}");
    }

    public virtual void Deactivate()
    {
        isActive = false;
        owner.canMove = true;
        Debug.Log($"{attackName}");
    }

    protected virtual void OnDrawGizmos()
    {
        if (isActive)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 1f);
        }
    }
}
