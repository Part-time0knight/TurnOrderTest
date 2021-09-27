using System.Collections;
using System.Collections.Generic;
public interface IWarriorsBox : IEnumerable<List<IWarrior>>
{ 
    int OrderSize { get; }

    int FractionCount { get; }

    void RemoveWarrior(IWarrior warrior);
}
