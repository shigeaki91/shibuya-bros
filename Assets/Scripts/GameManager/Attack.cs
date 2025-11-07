using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    public string attackName;
    public List<float> damage;
    public List<Vector2> knockback;
    public float occurTime;
    public float duration;
    public float endingLag;
    public float direction;
    public bool isActive;
    protected float timer;

    public Character owner;

    public virtual void Init(Character owner)
    {
        this.owner = owner;
        isActive = false;
    }

    public virtual void Activate()
    {
        direction = owner.sr.flipX ? -1f : 1f;
        isActive = true;
        owner.canMove = false;
        Debug.Log($"{attackName}");
    }

    public virtual void Deactivate()
    {
        isActive = false;
        owner.canMove = true;
        Debug.Log($"{attackName}" + " ended");
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
