using System.Collections.Generic;
using UnityEngine;
using R3;

public class WeakAttack : Attack
{
    public int attackCount = 0;
    public List<HitBox> hitBox;
    public WeakAttack(Character owner, Observable<Unit> attackInput, WeakAttackConfig config)
    {
        _attackName = config.AttackName;
        _damage[0] = config.Damage[0];
        _damage[1] = config.Damage[1];
        _damage[2] = config.Damage[2];
        _knockback[0] = config.Knockback[0];
        _knockback[1] = config.Knockback[1];
        _knockback[2] = config.Knockback[2];

        _occurTime = config.OccurTime;
        _duration = config.Duration;
        _endingLag = config.EndingLag;
        _attackInput = attackInput;
        for (int i = 0; i < 3; i++)
        {
            hitBox[i].owner = owner;
            hitBox[i].damage = _damage[i];
            hitBox[i].gameObject.SetActive(false);
        }
    }

    public override void Activate()
    {
        base.Activate();
        for (int i = 0; i < 3; i++)
        {
            hitBox[i].knockback = _knockback[i];
            hitBox[i].knockback.x = _knockback[i].x * _direction;
            Vector2 localPos = hitBox[i].transform.localPosition;
            localPos.x = Mathf.Abs(localPos.x) * _direction;
            hitBox[i].transform.localPosition = localPos;
        }

        owner.StartCoroutine(WeakAttackCoroutine0());
    }

    private System.Collections.IEnumerator WeakAttackCoroutine0()
    {
        owner.rb.linearVelocityX = 0f;
        yield return new WaitForSeconds(_occurTime);
        hitBox[0].gameObject.SetActive(true);
        float elapsed = 0f;
        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        hitBox[0].gameObject.SetActive(false);
        yield return new WaitForSeconds(_endingLag);
        if (attackCount >= 2 && hitBox[0].hit == true)
        {
            owner.StartCoroutine(WeakAttackCoroutine1());
            hitBox[0].hit = false;
        }
        else
        {
            Deactivate();
            attackCount = 0;
        }
    }

    private System.Collections.IEnumerator WeakAttackCoroutine1()
    {
        owner.rb.linearVelocityX = 0f;
        yield return new WaitForSeconds(_occurTime);
        hitBox[1].gameObject.SetActive(true);
        float elapsed = 0f;
        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        hitBox[1].gameObject.SetActive(false);
        yield return new WaitForSeconds(_endingLag);
        if (attackCount == 3) owner.StartCoroutine(WeakAttackCoroutine2());
        else
        {
            Deactivate();
            attackCount = 0;
        }
    }

    private System.Collections.IEnumerator WeakAttackCoroutine2()
    {
        owner.rb.linearVelocityX = 0f;
        yield return new WaitForSeconds(_occurTime);
        hitBox[2].gameObject.SetActive(true);
        float elapsed = 0f;
        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        hitBox[2].gameObject.SetActive(false);
        yield return new WaitForSeconds(_endingLag);
        Deactivate();
        attackCount = 0;
    }
}
