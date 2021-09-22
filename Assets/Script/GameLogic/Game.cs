using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    private TurnOrderView turnOrderView;
    private readonly GameLogic gameLogic = new GameLogic();
    private void Awake()
    {
        turnOrderView.Init(gameLogic, gameLogic);
        gameLogic.AddObserver(turnOrderView);
        
    }

    public void KillNextWarrior()
    {
        if (gameLogic.turnOrder.Count > 1)
            gameLogic.KillWarrior(1);
    }

    public void SkipTurn()
    {
        gameLogic.NextTurn();
    }
}
