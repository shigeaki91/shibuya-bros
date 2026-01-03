using R3;
using UnityEngine;

public class SelectButton
{
    public Observable<Unit> OnPressed {get; }

    public SelectButton(Observable<Unit> input)
    {
        OnPressed = input;
    }
}