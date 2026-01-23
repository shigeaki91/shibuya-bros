using UnityEngine;
using UnityEngine.InputSystem;
using Extensions;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] InputActionAsset _inputActionAsset;
    InputActionMap _inputActionMap;
    InputAction _selectAction;
    SelectButton _selectButton;
    [SerializeField] FadeController _fadeController;

    void Awake()
    {
        _inputActionMap = _inputActionAsset.FindActionMap("Title");
        _selectAction = _inputActionMap.FindAction("Select");
        _selectAction.Enable();
        var inputObservable = ObservableEx.InputActionAsObservable(_selectAction);
        _selectButton = new SelectButton(inputObservable);

        _selectButton.OnPressed
            .Subscribe(_ => OnSelectPressed().Forget())
            .AddTo(this);
    }

    async UniTask OnSelectPressed()
    {
        await _fadeController.FadeIn();
        SceneManager.LoadScene("CharaSelect");
    }


}