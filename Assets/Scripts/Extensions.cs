using UnityEngine;
using UnityEngine.InputSystem;
using R3;
using UnityEngine.UI;


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

        public static Observable<float> ChargeActionByObservable(InputAction action, float duration)
        {
            var pressing =
                Observable.Merge(
                        Observable.FromEvent<InputAction.CallbackContext>(
                            h => action.started += h,
                            h => action.started -= h
                        ).Select(_ => true),

                        Observable.FromEvent<InputAction.CallbackContext>(
                            h => action.canceled += h,
                            h => action.canceled -= h
                        ).Select(_ => false)
                    )
                    .Prepend(false);
                    
            return pressing
                .Select(isPressing =>
                {
                    if (isPressing)
                    {
                        return Observable.IntervalFrame(1)
                            .Select(_ => Time.deltaTime)
                            .Scan((acc, delta) => acc + delta)
                            .TakeWhile(elapsed => elapsed < duration)
                            .Concat(Observable.Return(duration))
                            .Select(elapsed => elapsed / duration);
                    }
                    else
                    {
                        return Observable.Return(0f);
                    }
                })
                .Switch();
        }
    }
}