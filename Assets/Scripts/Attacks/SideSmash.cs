using UnityEngine;
using R3;
using VContainer;

public class SideSmash : Attack
{
    HitBox _hitBox;
    AttackTypes _attackType = AttackTypes.SideSmash;
    public SideSmash(Character owner, Observable<Unit> attackInput, SideSmashConfig config,[Key(AttackTypes.SideSmash)] HitBox hitBox)
    {
        Init(owner);
        _attackName = config.AttackName;
        _damage = config.Damage;
        _knockback = config.Knockback;
        _occurTime = config.OccurTime;
        _duration = config.Duration;
        _endingLag = config.EndingLag;
        _attackInput = attackInput;
        _hitBox = hitBox;

        _hitBox.Owner = owner;
        _hitBox.Damage = _damage;
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

        _owner.StartCoroutine(SideSmashCoroutine());
    }

    private System.Collections.IEnumerator SideSmashCoroutine()
    {
        _owner.rb.linearVelocityX = 0f;
        _owner.Animator.SetTrigger("SideSmash");
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
