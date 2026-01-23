using UnityEngine;
using UnityEngine.UI;
using R3;

public class CharaSelectButton : MonoBehaviour
{
    Button _button;
    [SerializeField] CharacterNames _characterName;
    public Observable<Unit> OnClicked { get; private set; }

    void Awake()
    {
        _button = GetComponent<Button>();
        OnClicked = _button.OnClickAsObservable();
    }
}