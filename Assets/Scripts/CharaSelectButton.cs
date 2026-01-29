using UnityEngine;
using UnityEngine.UI;
using R3;

public class CharaSelectButton : MonoBehaviour
{
    public Button _button;
    public CharacterNames _characterName;
    public Sprite _characterImage;
    [SerializeField] Image _image;
    public Observable<Unit> OnClicked { get; private set; }

    void Awake()
    {
        _button = GetComponent<Button>();
        OnClicked = _button.OnClickAsObservable();
    }

    void Start()
    {
        _image.sprite = _characterImage;
    }
}