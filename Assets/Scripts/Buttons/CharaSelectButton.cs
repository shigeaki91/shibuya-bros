using UnityEngine;
using UnityEngine.UI;
using R3;

public class CharaSelectButton : MonoBehaviour
{
    public Button Button;
    public CharacterNames CharacterName;
    public Sprite CharacterImage;
    [SerializeField] Image _image;
    public Observable<Unit> OnClicked { get; private set; }

    void Awake()
    {
        Button = GetComponent<Button>();
        OnClicked = Button.OnClickAsObservable();
    }

    void Start()
    {
        _image.sprite = CharacterImage;
    }
}