using UnityEngine;
using R3;
using VContainer;
using System.Collections.Generic;

public class WeakAttack : Attack
{
    public int attackCount = 0;
    List<HitBox> _hitBox;
    Vector2[] _knockbacks = new Vector2[3];
    float[] _occurTimes = new float[3];
    float[] _durations = new float[3];
    float[] _endingLags = new float[3];
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
            _knockbacks[i] = config.Knockback[i];
        }
        _hitBox = hitBox;

        _occurTimes = config.OccurTime;
        _durations = config.Duration;
        _endingLags = config.EndingLag;
        _attackInput = attackInput;
        for (int i = 0; i < 3; i++)
        {
            _hitBox[i].Owner = _owner;
            _hitBox[i].AttackType = _attackTypes[i];
            _hitBox[i].Damage = config.Damage[i];
            _hitBox[i].DownTime = config.DownTime[i];
            _hitBox[i].gameObject.SetActive(false);
        }

        _attackInput
            .Where(_ =>  1 <= attackCount && attackCount < 3)
            .Subscribe(_ => {
                attackCount++;
            })
            .AddTo(_owner);
        _attackInput
            .Where(_ => _owner.GetAttackState() == _attackType && attackCount == 0)
            .Subscribe(_ => 
            {
                Activate();
                attackCount = 1;
            })
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
        _owner.Animator.SetTrigger("WeakAttack0");
        _owner.rb.linearVelocityX = 0f;
        yield return new WaitForSeconds(_occurTimes[0]);
        _hitBox[0].gameObject.SetActive(true);
        float elapsed = 0f;
        while (elapsed < _durations[0])
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        _hitBox[0].gameObject.SetActive(false);
        yield return new WaitForSeconds(_endingLags[0]);
        if (attackCount >= 2 && _hitBox[0].hit == true)
        {
            _owner.StartCoroutine(WeakAttackCoroutine1());
            _hitBox[0].hit = false;
        }
        else
        {
            Deactivate();
            attackCount = 0;
            Debug.Log("attackCount reset to 0" );
        }
    }

    private System.Collections.IEnumerator WeakAttackCoroutine1()
    {
        _owner.Animator.SetTrigger("WeakAttack1");
        _owner.rb.linearVelocityX = 0f;
        yield return new WaitForSeconds(_occurTimes[1]);
        _hitBox[1].gameObject.SetActive(true);
        float elapsed = 0f;
        while (elapsed < _durations[1])
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        _hitBox[1].gameObject.SetActive(false);
        yield return new WaitForSeconds(_endingLags[1]);
        if (attackCount == 3) _owner.StartCoroutine(WeakAttackCoroutine2());
        else
        {
            Deactivate();
            attackCount = 0;
        }
    }

    private System.Collections.IEnumerator WeakAttackCoroutine2()
    {
        _owner.Animator.SetTrigger("WeakAttack2");
        _owner.rb.linearVelocityX = 0f;
        yield return new WaitForSeconds(_occurTimes[2]);
        _hitBox[2].gameObject.SetActive(true);
        float elapsed = 0f;
        while (elapsed < _durations[2])
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        _hitBox[2].gameObject.SetActive(false);
        yield return new WaitForSeconds(_endingLags[2]);
        Deactivate();
        attackCount = 0;
    }
}
