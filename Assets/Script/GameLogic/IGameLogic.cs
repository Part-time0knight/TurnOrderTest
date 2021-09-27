using System.Collections.Generic;
public interface IGameLogic
{
    IWarrior CurrentWarrior { get; }
    int CurrentWarriorIndex { get; }

    int CurrentRound { get; }

    int CurrentRoundMod { get; }

    int OrderLength { get; }

    IReadOnlyList<List<IWarrior>> TurnOrder { get; }

    int Turn { get; }
}
