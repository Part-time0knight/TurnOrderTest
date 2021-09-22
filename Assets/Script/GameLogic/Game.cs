using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    private TurnOrderView _turnOrderView;

    private readonly GameUnits _gameUnits = new GameUnits();
    private GameLogic _gameLogic;

    public void KillNextWarrior()
    {
        if (_gameLogic.TurnOrder.Count > 1)
            _gameLogic.KillWarrior((1 + _gameLogic.CurrentWarriorIndex) % _gameLogic.TurnOrder.Count);
    }

    public void SkipTurn() => _gameLogic.NextTurn();

    private void Awake()
    {
        _gameLogic = new GameLogic(_gameUnits);
        _turnOrderView.Init(_gameLogic);
        _gameLogic.AddObserver(_turnOrderView);

    }
}
