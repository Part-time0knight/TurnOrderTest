using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnits
{
    public const int NUMBER_OF_FRACTION = 2;

    private readonly int[] speedBlueSet = { 6, 5, 5, 4, 3, 2, 1 };
    private readonly int[] speedRedSet = { 4, 4, 5, 3, 3, 4, 1 };
    private readonly int[] initiativeBlueSet = { 6, 8, 9, 8, 2, 4, 1 };
    private readonly int[] initiativeRedSet = { 8, 8, 9, 4, 2, 3, 1 };
    private readonly IFraction redFraction = new Fraction(new Color(1f, 0.5f, 0.5f), "R", 0);
    private readonly IFraction blueFraction = new Fraction(new Color(0.5f, 0.5f, 1f), "B", 1);
    private readonly List<IWarrior> warriors = new List<IWarrior>();
    public int orderSize => turnOrder.Count;
    public readonly List<IWarrior> turnOrder = new List<IWarrior>();

    public GameUnits()
    {
        for (int i = 0; i < 7; i++)
        {
            IWarrior warrior = new Warrior(redFraction, speedRedSet[i], initiativeRedSet[i], i);
            warriors.Add(warrior);
            AddInOrder(warrior);
            warrior = new Warrior(blueFraction, speedBlueSet[i], initiativeBlueSet[i], i);
            warriors.Add(warrior);
            AddInOrder(warrior);
        }
    }
    public void AddInOrder(IWarrior warrior)
    {
        bool end = false;
        int i;
        for (i = 0; !end && i < turnOrder.Count; i++ )
        {
            if (CompireWarriors(turnOrder[i], warrior))
            {
                end = true;
            }
        }
        if (end)
            turnOrder.Insert(--i, warrior);
        else
            turnOrder.Add(warrior);
    }
    private bool CompireWarriors(IWarrior warriorInOrder, IWarrior warrior)
        //возвращает true, когда warrior должен встать на место warriorInOrder, переместив второго ниже в очереди
    {
        //---проверка по инициативе
        if (warriorInOrder.initiative > warrior.initiative)
            return false;
        else if (warriorInOrder.initiative < warrior.initiative)
            return true;
        //---проверка по скорости
        if (warriorInOrder.speed > warrior.speed)
            return false;
        else if (warriorInOrder.speed < warrior.speed)
            return true;
        //---случай для воинов разной фракции
        if (warriorInOrder.fraction != warrior.fraction)
        {
            //---проверка по приоритету
            if ( ( turnOrder.IndexOf(warriorInOrder) + 1 ) % NUMBER_OF_FRACTION
                == warrior.fraction.priority)
                return true;
            else
                return false;
        }
        //---случай для воинов одной фракции
        else
        {
            //---проверка по номеру ячейки
            if (warriorInOrder.position < warrior.position)
                return false;
            else
                return true;
        }
    }
    public void RemoveWarrior(IWarrior warrior)
    {
        turnOrder.Remove(warrior);
    }
    public IWarrior GetWarrior(int index)
    {
        return turnOrder[index];
    }
}
