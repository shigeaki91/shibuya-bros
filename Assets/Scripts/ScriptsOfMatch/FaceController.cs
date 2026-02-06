using UnityEngine;
using R3;

public class FaceController : MonoBehaviour
{
    [SerializeField] SpriteRenderer faceSpriteRenderer;
    Animator _animator;
    void Awake()
    {
        _animator = GetComponentInParent<Animator>();
    }
}