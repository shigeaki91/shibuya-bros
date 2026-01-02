using UnityEngine;
using R3;
using VContainer;

public class DashAttack : Attack
{
    public float dashSpeed;
    HitBox _hitBox;
    AttackTypes _attackType = AttackTypes.DashAttack;
    public DashAttack(Character owner, Observable<Unit> attackInput, DashAttackConfig config, [Key(AttackTypes.DashAttack)] HitBox hitBox)
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

        dashSpeed = _owner.speed * 3f; // 初速設定
        
        _owner.StartCoroutine(DashAttackCoroutine(_direction));
    }

    private System.Collections.IEnumerator DashAttackCoroutine(float direction)
    {
        _owner.Animator.SetTrigger("DashAttack");
        yield return new WaitForSeconds(_occurTime);
        float elapsed = 0f;
        _hitBox.gameObject.SetActive(true);
        while (elapsed < _duration)
        {
            _owner.Move(direction, dashSpeed);
            elapsed += Time.deltaTime;
            dashSpeed -= _owner.speed * 2f / _duration * Time.deltaTime; // 最終的に元のスピードになるように。
            yield return null;
        }
        _hitBox.gameObject.SetActive(false);
        yield return new WaitForSeconds(_endingLag);
        Deactivate();
    }
}
