using UnityEngine;
using R3;
using VContainer;

public class AirSide : Attack
{
    HitBox _hitBox;
    AttackTypes _attackType = AttackTypes.AirSide;
    public AirSide(Character owner, Observable<Unit> attackInput, AirSideConfig config, [Key(AttackTypes.AirSide)] HitBox hitBox)
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
        _hitBox.transform.localPosition = localPos; //プレイヤーの向きが必要になるため、ノックバックはここで設定。
        
        _owner.StartCoroutine(AirSideCoroutine());
    }

    private System.Collections.IEnumerator AirSideCoroutine()
    {
        _owner.Animator.SetTrigger("AirSide");
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
