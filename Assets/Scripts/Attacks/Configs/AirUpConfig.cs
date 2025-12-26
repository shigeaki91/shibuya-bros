using UnityEngine;

[CreateAssetMenu(fileName = "AirUpConfig", menuName = "AttackConfigs/AirUpConfig")]
public class AirUpConfig : ScriptableObject
{
    [SerializeField] string _attackName = "Air Up";
    [SerializeField] float _damage = 7.2f;
    [SerializeField] Vector2 _knockback = new Vector2(0f, 6f);
    [SerializeField] float _occurTime = 0.1f;
    [SerializeField] float _duration = 0.2f;
    [SerializeField] float _endingLag = 0.4f;
    
    public string AttackName => _attackName;
    public float Damage => _damage;
    public Vector2 Knockback => _knockback;
    public float OccurTime => _occurTime;
    public float Duration => _duration;
    public float EndingLag => _endingLag;
}