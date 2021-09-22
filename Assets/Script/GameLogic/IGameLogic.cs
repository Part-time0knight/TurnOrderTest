using System.Collections.Generic;
public interface IGameLogic
{
    IReadOnlyList<IWarrior> TurnOrder { get; }

    int Turn { get; }
}
