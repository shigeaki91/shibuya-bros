using UnityEngine;

[CreateAssetMenu(fileName = "UpSmashConfig", menuName = "AttackConfigs/UpSmashConfig")]
public class UpSmashConfig : ScriptableObject
{
    [SerializeField] string _attackName = "Up Smash";
    [SerializeField] float _damage = 12.0f;
    [SerializeField] Vector2 _knockback = new Vector2(0f, 10f);
    [SerializeField] float _occurTime = 0.4f;
    [SerializeField] float _duration = 0.4f;
    [SerializeField] float _endingLag = 0.3f;

    public string AttackName => _attackName;
    public float Damage => _damage;
    public Vector2 Knockback => _knockback;
    public float OccurTime => _occurTime;
    public float Duration => _duration;
    public float EndingLag => _endingLag;
}