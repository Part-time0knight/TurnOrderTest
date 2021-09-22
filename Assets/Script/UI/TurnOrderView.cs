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

        List<IWarrior> order = new List<IWarrior>(gameLogic.TurnOrder);
        for (int i = 0; i < _viewSize; i++)
        {
            RoundPanel roundPanel = Instantiate(_roundPanel, _contentHider);
            _roundHide.Add(roundPanel);

            int roundIndex = i % order.Count;//когда он равен нулю, начинается новый раунд
            int roundNumber = i / order.Count;//номер раунда
            if (roundIndex == 0 && roundNumber > 0)
            {
                RoundPanelShow(roundNumber + 1);
            }
            
            WarriorPanel panel = Instantiate(_warriorPanel, _scrollRect.content);
            panel.WarriorSet(order[roundIndex]);
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
        for (int i = 0; i < _warriorPanels.Count; i++)
        {
            WarriorPanel item = _warriorPanels[i];
            if (item.Warrior == warrior)
            {
                _warriorPanels.Remove(item);
                Destroy(item.gameObject);
                i--;
            }
        }
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

        int roundIndex = (_gameLogic.Turn + _viewSize - 1) % _gameLogic.TurnOrder.Count;
        warrior = Instantiate(_warriorPanel, _scrollRect.content);
        warrior.WarriorSet(_gameLogic.TurnOrder[roundIndex]);
        _warriorPanels.Add(warrior);
    }

    private void ResetContent()
    {
        while (_roundShow.Count > 0)
            RoundPanelHide();

        for (int i = _warriorPanels.Count; i < _viewSize; i++)
        {
            _warriorPanels.Add(Instantiate(_warriorPanel, _scrollRect.content));
        }

        for (int i = 0; i < _warriorPanels.Count; i++)
        {
            int indexInOrder = (_gameLogic.Turn + i) % _gameLogic.TurnOrder.Count;
            _warriorPanels[i].WarriorSet(_gameLogic.TurnOrder[indexInOrder]);
        }

        Queue<WarriorPanel> tempList = new Queue<WarriorPanel>();
        for (int i = 0; i < _warriorPanels.Count; i++)
        {
            _warriorPanels[i].transform.SetParent(_contentHider);
            tempList.Enqueue(_warriorPanels[i]);
        }
        for (int i = 0; i < _warriorPanels.Count; i++)
        {
            bool isFirstPlace = i > 0 && (_gameLogic.Turn + i) % _gameLogic.TurnOrder.Count == 0;
            //возвращает истину когда ход воина - 1 в новом раунде
            if (isFirstPlace)
                RoundPanelShow((_gameLogic.Turn + i) / _gameLogic.TurnOrder.Count + 1);
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
