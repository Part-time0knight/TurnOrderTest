using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : IGameLogic, ILogicSubject
{
    public int turn => _turn;
    public List<IWarrior> turnOrder => _turnOrder;

    public IWarrior deadWarrior => _deadWarrior;

    private IWarrior _deadWarrior;
    private readonly List<IWarrior> _turnOrder = new List<IWarrior>();

    private int _turn = 0;

    private readonly GameUnits gameUnit = new GameUnits();
    private readonly List<ILogicObserver> logicObservers = new List<ILogicObserver>();
    public GameLogic()
    {
        for(int i = 0; i < gameUnit.orderSize; i++)
        {
            _turnOrder.Add(gameUnit.GetWarrior(i));
        }
    }
    public void KillWarrior(int index)
    {
        int turnIndex = (index + turn) % _turnOrder.Count;
        IWarrior warrior = _turnOrder[turnIndex];
        _turnOrder.RemoveAt(turnIndex);
        warrior.Death();
        gameUnit.RemoveWarrior(warrior);
        _deadWarrior = warrior;
        UpdateObserver(warrior);
    }
    public void NextTurn()
    {
        _turn++;
        UpdateObserver();
    }
    private void turnOrderUpdate()
    {
         
    }
    private void UpdateObserver()
    {
        foreach(ILogicObserver observer in logicObservers)
        {
            observer.LogicUpdate();
        }
    }
    private void UpdateObserver(IWarrior warrior)
    {
        foreach (ILogicObserver observer in logicObservers)
        {
            observer.KillWarrior(warrior);
        }
    }
    public void AddObserver(ILogicObserver logicObserver)
    {
        logicObservers.Add(logicObserver);
    }

    public void removeObserver(ILogicObserver logicObserver)
    {
        logicObservers.Remove(logicObserver);
    }
}
