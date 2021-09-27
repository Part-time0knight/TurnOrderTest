using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//класс, управляющий отображением очереди хода
[RequireComponent(typeof(ScrollRect))]
public class TurnOrderView : MonoBehaviour, ILogicObserver 
{
    [SerializeField]
    private WarriorPanel _warriorPanel;

    [SerializeField]
    private RoundPanel _roundPanel;

    [SerializeField]
    private int _viewSize = 20;

    [SerializeField]
    private Transform _contentHider;

    private ScrollRect _scrollRect;
    private IGameLogic _gameLogic;
    private readonly List<WarriorPanel> _warriorPanels = new List<WarriorPanel>();
    private readonly List<RoundPanel> _roundHide = new List<RoundPanel>();
    private readonly List<RoundPanel> _roundShow = new List<RoundPanel>();
    public void Init(IGameLogic gameLogic)
    {
        if (!_scrollRect)
            _scrollRect = GetComponent<ScrollRect>();

        _gameLogic = gameLogic;

        List<List<IWarrior>> order = new List<List<IWarrior>>(gameLogic.TurnOrder);
        int round = gameLogic.CurrentRound;
        for (int i = 0; i < _viewSize; i++)
        {
            RoundPanel roundPanel = Instantiate(_roundPanel, _contentHider);
            _roundHide.Add(roundPanel);

            int roundIndex = i % _gameLogic.OrderLength;//когда он равен нулю, начинается новый раунд
            //int roundNumber = gameLogic.CurrentRound + i / order[gameLogic.CurrentRound].Count;//номер раунда
            if (roundIndex == 0 && round++ > 0)
            {
                RoundPanelShow(round);
            }
            
            WarriorPanel panel = Instantiate(_warriorPanel, _scrollRect.content);
            panel.WarriorSet(order[(round - 1) % order.Count][roundIndex]);
            _warriorPanels.Add(panel);
            panel.Turn = i + 1;
        }
    }

    public void LogicUpdate()
    {
        MakeTurn();
        UpdateItems();
    }

    public void KillWarrior(IWarrior warrior)
    {
        UpdateItems();
    }

    private void RoundPanelShow(int round)
    {
        _roundShow.Add(_roundHide[0]);
        _roundHide[0].transform.SetParent(_scrollRect.content);
        _roundHide[0].RoundSet(round);
        _roundHide[0].UpdateText();
        _roundHide.RemoveAt(0);
    }

    private void RoundPanelHide()
    {
        RoundPanel panel = _roundShow[0];
        _roundShow.RemoveAt(0);
        _roundHide.Add(panel);
        panel.transform.SetParent(_contentHider);
    }

    private void MakeTurn()
    {
        WarriorPanel warrior = _warriorPanels[0];
        _warriorPanels.RemoveAt(0);
        Destroy(warrior.gameObject);

        int roundIndex = (_gameLogic.Turn + _viewSize - 1) % _gameLogic.OrderLength;
        int roundMod = (_gameLogic.CurrentRoundMod + (_gameLogic.Turn + _viewSize - 1) / _gameLogic.OrderLength) % _gameLogic.TurnOrder.Count;
        warrior = Instantiate(_warriorPanel, _scrollRect.content);
        warrior.WarriorSet(_gameLogic.TurnOrder[roundMod][roundIndex]);
        _warriorPanels.Add(warrior);
    }

    private void ResetContent()
    {
        while (_roundShow.Count > 0)
            RoundPanelHide();

        for (int i = 0; i < _warriorPanels.Count; i++)
        {
            int indexInOrder = (_gameLogic.CurrentWarriorIndex + i) % (_gameLogic.OrderLength);
            int roundMod = ((_gameLogic.Turn + i) / _gameLogic.OrderLength) % _gameLogic.TurnOrder.Count;
            _warriorPanels[i].WarriorSet(_gameLogic.TurnOrder[roundMod][indexInOrder]);
        }

        Queue<WarriorPanel> tempList = new Queue<WarriorPanel>();
        for (int i = 0; i < _warriorPanels.Count; i++)
        {
            _warriorPanels[i].transform.SetParent(_contentHider);
            tempList.Enqueue(_warriorPanels[i]);
        }

        int nextRound = 1;
        for (int i = 0; i < _warriorPanels.Count; i++)
        {
            bool isFirstPlace = i > 0 && (_gameLogic.CurrentWarriorIndex + i) % _gameLogic.OrderLength == 0;
            //возвращает истину когда ход воина - первый в новом раунде
            if (isFirstPlace)
            {
                int round = _gameLogic.CurrentRound + ++nextRound;
                RoundPanelShow(round);
            }
            tempList.Dequeue().transform.SetParent(_scrollRect.content);
        }
    }

    private void UpdateItems()
    {
        ResetContent();
        for (int i = 0; i < _viewSize; i++)
        {
            WarriorPanel panel = _warriorPanels[i];
            panel.Turn = i + 1 + _gameLogic.Turn;
        }
    }
}
