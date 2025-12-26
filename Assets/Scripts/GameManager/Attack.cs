using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using R3;

public abstract class Attack
{
    public Observable<Unit> _attackInput;
    public string _attackName;
    public List<float> _damage;
    public List<Vector2> _knockback;
    public float _occurTime;
    public float _duration;
    public float _endingLag;
    public float _direction;
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
        _direction = owner.sr.flipX ? -1f : 1f;
        isActive = true;
        owner.canMove = false;
        Debug.Log($"{_attackName}");
    }

    public virtual void Deactivate()
    {
        isActive = false;
        owner.canMove = true;
        Debug.Log($"{_attackName}" + " ended");
    }
}
