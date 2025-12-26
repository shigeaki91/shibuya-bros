using UnityEngine;
using UnityEngine.InputSystem;
using R3;

namespace Extensions
{
    public static class ObservableEx
    {
        public static Observable<Unit> InputActionAsObservable(InputAction action)
        {
            return Observable.FromEvent<InputAction.CallbackContext>(
                h => action.performed += h,
                h => action.performed -= h
            )
            .Select(_ => Unit.Default);
        }
    }
}