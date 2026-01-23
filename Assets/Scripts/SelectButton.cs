using R3;

public class SelectButton
{
    public Observable<Unit> OnPressed {get; }

    public SelectButton(Observable<Unit> input)
    {
        OnPressed = input;
    }
}