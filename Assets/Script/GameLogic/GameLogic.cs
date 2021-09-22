using System.Collections.Generic;

public class GameLogic : IGameLogic, ILogicSubject
{
    public int Turn => _turn;
    private int _turn = 0;

    public IReadOnlyList<IWarrior> TurnOrder => _turnOrder;
    private List<IWarrior> _turnOrder = new List<IWarrior>();

    private readonly IWarriorsBox _gameUnit;

    private readonly List<ILogicObserver> _logicObservers = new List<ILogicObserver>();

    public GameLogic(IWarriorsBox warriorsBox)
    {
        _gameUnit = warriorsBox;
        _turnOrder = new List<IWarrior>(_gameUnit);
    }

    public void KillWarrior(int index)
    {
        IWarrior warrior = _turnOrder[index];
        warrior.Death();
        _gameUnit.RemoveWarrior(warrior);
        _turnOrder = new List<IWarrior>(_gameUnit);
        UpdateObserver(warrior);
    }

    public void NextTurn()
    {
        _turn++;
        UpdateObserver();
    }

    public void AddObserver(ILogicObserver logicObserver) => _logicObservers.Add(logicObserver);

    public void removeObserver(ILogicObserver logicObserver) => _logicObservers.Remove(logicObserver);

    private void UpdateObserver()
    {
        foreach (ILogicObserver observer in _logicObservers)
        {
            observer.LogicUpdate();
        }
    }

    private void UpdateObserver(IWarrior warrior)
    {
        foreach (ILogicObserver observer in _logicObservers)
        {
            observer.KillWarrior(warrior);
        }
    }
}
