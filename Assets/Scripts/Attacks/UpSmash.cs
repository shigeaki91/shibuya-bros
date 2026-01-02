using UnityEngine;
using R3;
using VContainer;

public class UpSmash : Attack
{
    HitBox _hitBox;
    AttackTypes _attackType = AttackTypes.UpSmash;
    public UpSmash(Character owner, Observable<Unit> attackInput, UpSmashConfig config, [Key(AttackTypes.UpSmash)] HitBox hitBox)
    {
        Init(owner);
        _attackName = config.AttackName;
        _knockback = config.Knockback;
        _occurTime = config.OccurTime;
        _duration = config.Duration;
        _endingLag = config.EndingLag;
        _attackInput = attackInput;
        _hitBox = hitBox;

        _hitBox.Owner = _owner;
        _hitBox.Damage = config.Damage;
        _hitBox.DownTime = config.DownTime;
        _hitBox.gameObject.SetActive(false);

        _attackInput
            .Where(_ => _owner.GetAttackState() == _attackType)
            .Subscribe(_ => Activate())
            .AddTo(_owner);
    }

    public override void Activate()
    {
        base.Activate();
        _hitBox.Knockback = _knockback;
        _hitBox.Knockback.x = _knockback.x * _direction;
        Vector2 localPos = _hitBox.transform.localPosition;
        localPos.x = Mathf.Abs(localPos.x) * _direction;
        _hitBox.transform.localPosition = localPos;

        _owner.StartCoroutine(UpSmashCoroutine());
    }

    private System.Collections.IEnumerator UpSmashCoroutine()
    {
        _owner.Animator.SetTrigger("UpSmash");
        _owner.rb.linearVelocityX = 0f;
        yield return new WaitForSeconds(_occurTime);
        _hitBox.gameObject.SetActive(true);
        float elapsed = 0f;
        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        _hitBox.gameObject.SetActive(false);
        yield return new WaitForSeconds(_endingLag);
        Deactivate();
    }
}
