using UnityEngine;
using R3;
using VContainer;

public class AirDown : Attack
{
    HitBox _hitBox;
    AttackTypes _attackType = AttackTypes.AirDown;
    public AirDown(Character owner, Observable<Unit> attackInput, AirDownConfig config, [Key(AttackTypes.AirDown)] HitBox hitBox)
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
        
        _hitBox.Owner = _owner;
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
        
        _owner.StartCoroutine(AirDownCoroutine());
    }

    private System.Collections.IEnumerator AirDownCoroutine()
    {
        _owner.Animator.SetTrigger("AirLow");
        yield return new WaitForSeconds(_occurTime);
        float elapsed = 0f;
        if (!_owner.isGrounded) _hitBox.gameObject.SetActive(true);
        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
            if (_owner.isGrounded)
            {
                break;
            }
        }
        _hitBox.gameObject.SetActive(false);
        yield return new WaitForSeconds(_endingLag);
        Deactivate();
    }
}
