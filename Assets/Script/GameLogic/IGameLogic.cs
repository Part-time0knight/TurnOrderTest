using System.Collections.Generic;
public interface IGameLogic
{
    IWarrior CurrentWarrior { get; }
    int CurrentWarriorIndex { get; }

    int CurrentRound { get; }
    IReadOnlyList<IWarrior> TurnOrder { get; }

    int Turn { get; }
}
