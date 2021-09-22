using System.Collections.Generic;

public class GameLogic : IGameLogic, ILogicSubject
{
    public int Turn => _turn;
    private int _turn = 0;

    public IReadOnlyList<IWarrior> TurnOrder => _turnOrder;

    public IWarrior CurrentWarrior => _currentWarrior;
    private IWarrior _currentWarrior;
    public int CurrentWarriorIndex => _currentWarriorIndex;

    public int CurrentRound => _round;
    private int _round = 0;

    private int _currentWarriorIndex = 0;

    private List<IWarrior> _turnOrder = new List<IWarrior>();

    private readonly IWarriorsBox _gameUnit;

    private readonly List<ILogicObserver> _logicObservers = new List<ILogicObserver>();

    public GameLogic(IWarriorsBox warriorsBox)
    {
        _gameUnit = warriorsBox;
        _turnOrder = new List<IWarrior>(_gameUnit);
        _currentWarrior = _turnOrder[0];
    }

    public void KillWarrior(int index)
    {
        IWarrior warrior = _turnOrder[index];
        if (warrior == _currentWarrior)
        {
            _currentWarriorIndex = ++index % _turnOrder.Count;
            _currentWarrior = _turnOrder[_currentWarriorIndex];
        }
        _gameUnit.RemoveWarrior(warrior);
        _turnOrder = new List<IWarrior>(_gameUnit);
        _currentWarriorIndex = _turnOrder.IndexOf(_currentWarrior);
        ObserverKillWarrior(warrior);
        warrior.Death();
    }

    public void NextTurn()
    {
        if (++_turn % _turnOrder.Count == 0) _round++;
        _currentWarriorIndex = ++_currentWarriorIndex % _turnOrder.Count;
        _currentWarrior = _turnOrder[_currentWarriorIndex];
        UpdateObserver();
    }

    public void AddObserver(ILogicObserver logicObserver) => _logicObservers.Add(logicObserver);

    public void RemoveObserver(ILogicObserver logicObserver) => _logicObservers.Remove(logicObserver);

    private void UpdateObserver()
    {
        foreach (ILogicObserver observer in _logicObservers)
        {
            observer.LogicUpdate();
        }
    }

    private void ObserverKillWarrior(IWarrior warrior)
    {
        foreach (ILogicObserver observer in _logicObservers)
        {
            observer.KillWarrior(warrior);
        }
    }
}
