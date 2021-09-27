using System.Collections.Generic;

public class GameLogic : IGameLogic, ILogicSubject
{
    public int Turn => _turn;
    private int _turn = 0;

    public IReadOnlyList<List<IWarrior>> TurnOrder => _turnOrder;

    public IWarrior CurrentWarrior => _currentWarrior;
    private IWarrior _currentWarrior;
    public int CurrentWarriorIndex => _currentWarriorIndex;

    public int CurrentRound => _round;
    private int _round = 0;

    public int CurrentRoundMod => _roundMod;
    private int _roundMod = 0;

    public int OrderLength => _turnOrder[0].Count;

    private int _currentWarriorIndex = 0;

    private List<List<IWarrior>> _turnOrder;

    private readonly IWarriorsBox _gameUnit;

    private readonly List<ILogicObserver> _logicObservers = new List<ILogicObserver>();

    public GameLogic(IWarriorsBox warriorsBox)
    {
        _gameUnit = warriorsBox;
        _turnOrder = new List<List<IWarrior>>(_gameUnit);
        _currentWarrior = _turnOrder[0][0];
    }

    public void KillWarrior(int index)
    {
        IWarrior warrior = _turnOrder[_roundMod][index];
        if (warrior == _currentWarrior)
        {
            _currentWarriorIndex = ++index % _turnOrder[_roundMod].Count;
            _currentWarrior = _turnOrder[_roundMod][_currentWarriorIndex];
        }
        _gameUnit.RemoveWarrior(warrior);
        _turnOrder =new List<List<IWarrior>>(_gameUnit);
        _currentWarriorIndex = _turnOrder[_roundMod].IndexOf(_currentWarrior);
        ObserverKillWarrior(warrior);
        warrior.Death();
    }

    public void NextTurn()
    {
        if (++_turn % _turnOrder[_roundMod].Count == 0) { _roundMod = ++_round % _turnOrder.Count; }
        _currentWarriorIndex = ++_currentWarriorIndex % _turnOrder[_roundMod].Count;
        _currentWarrior = _turnOrder[_roundMod][_currentWarriorIndex];
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
