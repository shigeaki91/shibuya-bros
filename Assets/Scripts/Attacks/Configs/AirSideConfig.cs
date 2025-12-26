using UnityEngine;

[CreateAssetMenu(fileName = "AirSideConfig", menuName = "AttackConfigs/AirSideConfig")]
public class AirSideConfig : ScriptableObject
{
    [SerializeField] string _attackName = "Air Side";
    [SerializeField] float _damage = 6.8f;
    [SerializeField] Vector2 _knockback = new Vector2(4f, 2f);
    [SerializeField] float _occurTime = 0.2f;
    [SerializeField] float _duration = 0.2f;
    [SerializeField] float _endingLag = 0.3f;
    
    public string AttackName => _attackName;
    public float Damage => _damage;
    public Vector2 Knockback => _knockback;
    public float OccurTime => _occurTime;
    public float Duration => _duration;
    public float EndingLag => _endingLag;
}