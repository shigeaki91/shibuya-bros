using System.Collections.Generic;
using UnityEngine;
using R3;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

[System.Serializable]
struct CharaImageEntry
{
    public CharacterNames CharacterName;
    public Sprite CharacterImage;
}
public class SelectManager : MonoBehaviour
{
    public int _selectedIndex = 0;
    [SerializeField] Canvas _canvas;
    [SerializeField] GameObject _charaSelectButtonPrefab;
    [SerializeField] List<CharaSelectButton> _charaSelectButtons;
    [SerializeField] CharaImageEntry[] _charaImageEntries;
    Dictionary<CharacterNames, Sprite> _charaImageDict;
    [SerializeField] List<Vector2> _buttonPosition;

    [SerializeField] InputActionAsset _inputActionAsset;
    InputActionMap _inputActionMap;
    InputAction _backAction;
    SelectButton _backButton;
    [SerializeField] FadeController _fadeController;

    void Awake()
    {
        var i = 0;
        var charaNumber = System.Enum.GetValues(typeof(CharacterNames)).Length;
        _charaImageDict = new Dictionary<CharacterNames, Sprite>();
        foreach (var entry in _charaImageEntries)
        {
            _charaImageDict[entry.CharacterName] = entry.CharacterImage;

            var button = Instantiate(_charaSelectButtonPrefab, _canvas.transform).GetComponent<CharaSelectButton>();

            _charaSelectButtons.Add(button);
            button._characterName = entry.CharacterName;
            button._characterImage = entry.CharacterImage;
            button.transform.localPosition = _buttonPosition[i];
            i++;

            button.OnClicked.Subscribe(_ =>
            {
                Debug.Log($"{entry.CharacterName} selected.");
                _selectedIndex += 1;
            }).AddTo(this);
        }

        _inputActionMap = _inputActionAsset.FindActionMap("CharaSelect");
        _backAction = _inputActionMap.FindAction("Back");
        _backAction.Enable();
        var inputObservable = Extensions.ObservableEx.InputActionAsObservable(_backAction);
        _backButton = new SelectButton(inputObservable);

        _backButton.OnPressed
            .Subscribe(_ => OnBackPressed())
            .AddTo(this);

        var chargeObservable = Extensions.ObservableEx.ChargeActionByObservable(_backAction, 2.0f);
        chargeObservable
            .Where(charge => charge >= 1.0f && _selectedIndex == 0)
            .Subscribe(_ => GoBackToTitle().Forget())
            .AddTo(this);
    }

    void OnBackPressed()
    {
        Debug.Log("Go Back One");
        if (_selectedIndex > 0)
        {
            _selectedIndex -= 1;
        }
    }
    async UniTask GoBackToTitle()
    {
        await _fadeController.FadeIn();
        SceneManager.LoadScene("Title");
    }
}