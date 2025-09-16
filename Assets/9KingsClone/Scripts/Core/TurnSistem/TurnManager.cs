using System;
using UniRx;

public class TurnManager
{
    public IReadOnlyReactiveProperty<int> CurrentTurn => _currentTurn;
    private readonly ReactiveProperty<int> _currentTurn = new(1);

    public IObservable<int> OnTurnStart => _onTurnStart;
    public IObservable<int> OnTurnEnd => _onTurnEnd;

    private readonly Subject<int> _onTurnStart = new();
    private readonly Subject<int> _onTurnEnd = new();

    public void NextTurn()
    {
        _onTurnEnd.OnNext(_currentTurn.Value);
        _currentTurn.Value++;
        _onTurnStart.OnNext(_currentTurn.Value);
    }
}
