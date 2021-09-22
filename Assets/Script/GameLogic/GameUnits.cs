using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnits : IWarriorsBox
{
    //данные о солдатах были захардкожены для ускорения выполнения тестового
    //при подключении сторонних систем нужно будет заменять класс GameUnits
    public const int NUMBER_OF_FRACTION = 2;
    public const int NUMBER_OF_SOLDIER = 7;

    private readonly int[] _speedBlueSet = { 6, 5, 5, 4, 3, 2, 1 };
    private readonly int[] _speedRedSet = { 4, 4, 5, 3, 3, 4, 1 };
    private readonly int[] _initiativeBlueSet = { 6, 8, 9, 8, 2, 4, 1 };
    private readonly int[] _initiativeRedSet = { 8, 8, 9, 4, 2, 3, 1 };

    private readonly IFraction _redFraction = new Fraction(new Color(1f, 0.5f, 0.5f), "R", 0);
    private readonly IFraction _blueFraction = new Fraction(new Color(0.5f, 0.5f, 1f), "B", 1);
    private readonly List<IWarrior> _warriors = new List<IWarrior>();

    public int OrderSize => _turnOrder.Count;
    private readonly List<IWarrior> _turnOrder = new List<IWarrior>();

    public GameUnits()
    {
        for (int i = 0; i < NUMBER_OF_SOLDIER; i++)
        {
            IWarrior warrior = new Warrior(_redFraction, _speedRedSet[i], _initiativeRedSet[i], i, i);
            _warriors.Add(warrior);
            AddInOrder(warrior);
            warrior = new Warrior(_blueFraction, _speedBlueSet[i], _initiativeBlueSet[i], i, i + NUMBER_OF_SOLDIER);
            _warriors.Add(warrior);
            AddInOrder(warrior);
        }
    }
    public void AddInOrder(IWarrior warrior)
    {
        bool end = false;
        int i;
        for (i = 0; !end && i < _turnOrder.Count; i++ )
        {
            if (CompireWarriors(_turnOrder[i], warrior))
            {
                end = true;
            }
        }
        if (end)
            _turnOrder.Insert(--i, warrior);
        else
            _turnOrder.Add(warrior);
    }
    public void RemoveWarrior(IWarrior warrior)
    {
        _turnOrder.Clear();
        _warriors.Remove(warrior);
        foreach (IWarrior item in _warriors)
        {
            AddInOrder(item);
        }
    }

    public IWarrior GetWarrior(int index) => _turnOrder[index];

    public IEnumerator<IWarrior> GetEnumerator() => _turnOrder.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private bool CompireWarriors(IWarrior warriorInOrder, IWarrior warrior)
    //возвращает true, когда warrior должен встать на место warriorInOrder, переместив второго ниже в очереди
    {
        //---проверка по инициативе
        if (warriorInOrder.Initiative > warrior.Initiative)
            return false;
        if (warriorInOrder.Initiative < warrior.Initiative)
            return true;
        //---проверка по скорости
        if (warriorInOrder.Speed > warrior.Speed)
            return false;
        if (warriorInOrder.Speed < warrior.Speed)
            return true;
        //---случай для воинов разной фракции
        if (warriorInOrder.Fraction != warrior.Fraction)
        {
            //---проверка по приоритету
            bool fractionOrder = (_turnOrder.IndexOf(warriorInOrder) + 1) % NUMBER_OF_FRACTION
                == warrior.Fraction.Priority;
            //в зависимости от четности/нечетности позиции ходят красные или синие
            if (fractionOrder)
                return true;
            return false;
        }
        //---случай для воинов одной фракции
            //---проверка по номеру ячейки
        if (warriorInOrder.Position < warrior.Position)
            return false;
        return true;
    }

}
