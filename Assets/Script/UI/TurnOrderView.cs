using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//класс, управляющий отображением очереди хода
[RequireComponent(typeof(ScrollRect))]
public class TurnOrderView : MonoBehaviour, ILogicObserver 
{
    [SerializeField]
    private WarriorPanel warriorPanel;
    [SerializeField]
    private RoundPanel roundPanel;
    [SerializeField]
    private int viewSize = 20;
    [SerializeField]
    private Transform contentHider;
    private ScrollRect scrollRect;
    private IGameLogic gameLogic;
    private int turn = 0;
    private ILogicSubject logicSubject;
    private readonly List<WarriorPanel> warriorPanels = new List<WarriorPanel>();
    private readonly List<RoundPanel> roundHide = new List<RoundPanel>();
    private readonly List<RoundPanel> roundShow = new List<RoundPanel>();
    public void Init(IGameLogic gameLogic, ILogicSubject logicSubject)
    {
        if (!scrollRect)
            scrollRect = GetComponent<ScrollRect>();
        this.gameLogic = gameLogic;
        this.logicSubject = logicSubject;
        List<IWarrior> order = gameLogic.turnOrder;
        for (int i = 0; i < viewSize; i++)
        {
            RoundPanel roundPanel = Instantiate(this.roundPanel, contentHider);
            roundHide.Add(roundPanel);
        }
        for (int i = 0; i < viewSize; i++)
        {
            int roundIndex = i % order.Count;
            if (roundIndex == 0 && i / order.Count > 0)
            {
                RoundPanelShow(i / order.Count + 1);
            }
            WarriorPanel panel = Instantiate(warriorPanel, scrollRect.content);
            panel.WarriorSet(order[roundIndex]);
            warriorPanels.Add(panel);
            panel.turn = i + 1;
        }
    }
    private void RoundPanelShow(int round)
    {
        roundShow.Add(roundHide[0]);
        roundHide[0].transform.SetParent(scrollRect.content);
        roundHide[0].RoundSet(round);
        roundHide[0].UpdateText();
        roundHide.RemoveAt(0);
    }
    private void RoundPanelHide()
    {
        RoundPanel panel = roundShow[0];
        roundShow.RemoveAt(0);
        roundHide.Add(panel);
        panel.transform.SetParent(contentHider);
    }
    public void LogicUpdate()
    {
        MakeTurn();
        UpdateItems();
    }
    private void MakeTurn()
    {
        if (gameLogic.turn % gameLogic.turnOrder.Count == 0)
            RoundPanelHide();
        if ((gameLogic.turn + viewSize - 1) % gameLogic.turnOrder.Count == 0)
            RoundPanelShow((gameLogic.turn + viewSize) / gameLogic.turnOrder.Count + 1);
        WarriorPanel warrior = warriorPanels[0];
        warriorPanels.RemoveAt(0);
        Destroy(warrior.gameObject);

        int roundIndex = (gameLogic.turn + viewSize - 1) % gameLogic.turnOrder.Count;
        warrior = Instantiate(warriorPanel, scrollRect.content);
        warrior.WarriorSet(gameLogic.turnOrder[roundIndex]);
        warriorPanels.Add(warrior);
        turn = gameLogic.turn;
    }
    private void UpdateItems()
    {
        for (int i = 0; i < viewSize; i++)
        {
            WarriorPanel panel = warriorPanels[i];
            panel.turn = i + 1 + gameLogic.turn;
        }
    }

    public void KillWarrior(IWarrior warrior)
    {
        while (roundShow.Count > 0)
            RoundPanelHide();
        int deleteCount = 0;
        for (int i = 0; i < warriorPanels.Count; i++)
        {
            WarriorPanel item = warriorPanels[i];
            if (item.warrior == warrior)
            {
                warriorPanels.Remove(item);
                Destroy(item.gameObject);
                int roundIndex = (gameLogic.turn + viewSize - 2 + deleteCount++) % gameLogic.turnOrder.Count;
                WarriorPanel panel = Instantiate(warriorPanel, scrollRect.content);
                panel.WarriorSet(gameLogic.turnOrder[roundIndex]);
                warriorPanels.Add(panel);
                i--;
            }

        }
        Queue<WarriorPanel> tempList = new Queue<WarriorPanel>();
        for (int i = 0; i < warriorPanels.Count; i++)
        {
            warriorPanels[i].transform.SetParent(contentHider);
            tempList.Enqueue(warriorPanels[i]);
        }
        for (int i = 0; i < warriorPanels.Count; i++)
        {
            if (i > 0 && (gameLogic.turn + i) % gameLogic.turnOrder.Count == 0)
                RoundPanelShow((gameLogic.turn + i) / gameLogic.turnOrder.Count + 1);
            tempList.Dequeue().transform.SetParent(scrollRect.content);
        }

        UpdateItems();

    }
}
