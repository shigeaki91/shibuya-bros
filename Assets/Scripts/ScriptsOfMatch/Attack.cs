using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using R3;

public abstract class Attack
{
    public Observable<Unit> _attackInput;
    public string _attackName;
    public Vector2 _knockback;
    public float _occurTime;
    public float _duration;
    public float _endingLag;
    public float _direction;
    public bool isActive;

    protected float timer;

    protected Character _owner;

    public virtual void Init(Character owner)
    {
        _owner = owner;
        isActive = false;
    }

    public virtual void Activate()
    {
        _direction = _owner.sr.flipX ? -1f : 1f;
        isActive = true;
        _owner.isAttacking = true;
        _owner.Animator.SetBool("Idling", false);
        Debug.Log($"{_attackName}");
    }

    public virtual void Deactivate()
    {
        isActive = false;
        _owner.isAttacking = false;
        _owner.Animator.SetBool("Idling", true);
        Debug.Log($"{_attackName}" + " ended");
    }
}
