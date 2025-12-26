using UnityEngine;

[CreateAssetMenu(fileName = "AirNeutralConfig", menuName = "AttackConfigs/AirNeutralConfig")]
public class AirNeutralConfig : ScriptableObject
{
    [SerializeField] string _attackName = "Air Neutral";
    [SerializeField] float _damage = 4.6f;
    [SerializeField] Vector2 _knockback = new Vector2(3f, 2f);
    [SerializeField] float _occurTime = 0.1f;
    [SerializeField] float _duration = 0.6f;
    [SerializeField] float _endingLag = 0.15f;
    
    public string AttackName => _attackName;
    public float Damage => _damage;
    public Vector2 Knockback => _knockback;
    public float OccurTime => _occurTime;
    public float Duration => _duration;
    public float EndingLag => _endingLag;
}