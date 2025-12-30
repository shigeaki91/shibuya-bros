using UnityEngine;
using R3;
using VContainer;
using System.Collections.Generic;

public class WeakAttack : Attack
{
    public int attackCount = 0;
    List<HitBox> _hitBox;
    float[] _damages = new float[3];
    Vector2[] _knockbacks = new Vector2[3];
    AttackTypes _attackType = AttackTypes.WeakAttack;
    AttackTypes[] _attackTypes = new AttackTypes[3]
    {
        AttackTypes.WeakAttack0,
        AttackTypes.WeakAttack1,
        AttackTypes.WeakAttack2
    };
    public WeakAttack(Character owner, Observable<Unit> attackInput, WeakAttackConfig config, [Key(AttackTypes.WeakAttack)]  List<HitBox> hitBox)
    {
        Init(owner);
        _attackName = config.AttackName;
        for (int i = 0; i < 3; i++)
        {
            _damages[i] = config.Damage[i];
            _knockbacks[i] = config.Knockback[i];
        }
        _hitBox = hitBox;

        _occurTime = config.OccurTime;
        _duration = config.Duration;
        _endingLag = config.EndingLag;
        _attackInput = attackInput;
        for (int i = 0; i < 3; i++)
        {
            _hitBox[i].Owner = owner;
            _hitBox[i].AttackType = _attackTypes[i];
            _hitBox[i].Damage = _damages[i];
            _hitBox[i].gameObject.SetActive(false);
        }

        _attackInput
            .Where(_ =>  attackCount < 3)
            .Subscribe(_ => {
                attackCount++;
            })
            .AddTo(_owner);
        _attackInput
            .Where(_ => _owner.GetAttackState() == _attackType)
            .Subscribe(_ => Activate())
            .AddTo(_owner);
    }

    public override void Activate()
    {
        base.Activate();
        for (int i = 0; i < 3; i++)
        {
            _hitBox[i].Knockback = _knockbacks[i];
            _hitBox[i].Knockback.x = _knockbacks[i].x * _direction;
            Vector2 localPos = _hitBox[i].transform.localPosition;
            localPos.x = Mathf.Abs(localPos.x) * _direction;
            _hitBox[i].transform.localPosition = localPos;
        }

        _owner.StartCoroutine(WeakAttackCoroutine0());
    }

    private System.Collections.IEnumerator WeakAttackCoroutine0()
    {
        _owner.rb.linearVelocityX = 0f;
        yield return new WaitForSeconds(_occurTime);
        _hitBox[0].gameObject.SetActive(true);
        float elapsed = 0f;
        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        _hitBox[0].gameObject.SetActive(false);
        yield return new WaitForSeconds(_endingLag);
        if (attackCount >= 2 && _hitBox[0].hit == true)
        {
            _owner.StartCoroutine(WeakAttackCoroutine1());
            _hitBox[0].hit = false;
        }
        else
        {
            Deactivate();
            attackCount = 0;
        }
    }

    private System.Collections.IEnumerator WeakAttackCoroutine1()
    {
        _owner.rb.linearVelocityX = 0f;
        yield return new WaitForSeconds(_occurTime);
        _hitBox[1].gameObject.SetActive(true);
        float elapsed = 0f;
        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        _hitBox[1].gameObject.SetActive(false);
        yield return new WaitForSeconds(_endingLag);
        if (attackCount == 3) _owner.StartCoroutine(WeakAttackCoroutine2());
        else
        {
            Deactivate();
            attackCount = 0;
        }
    }

    private System.Collections.IEnumerator WeakAttackCoroutine2()
    {
        _owner.rb.linearVelocityX = 0f;
        yield return new WaitForSeconds(_occurTime);
        _hitBox[2].gameObject.SetActive(true);
        float elapsed = 0f;
        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        _hitBox[2].gameObject.SetActive(false);
        yield return new WaitForSeconds(_endingLag);
        Deactivate();
        attackCount = 0;
    }
}
