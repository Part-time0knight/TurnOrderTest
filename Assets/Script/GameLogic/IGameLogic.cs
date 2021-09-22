using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IGameLogic
{
    public IWarrior deadWarrior { get; }
    public List<IWarrior> turnOrder { get; }
    public int turn { get; }
}
