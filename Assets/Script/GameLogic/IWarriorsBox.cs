using System.Collections;
using System.Collections.Generic;
public interface IWarriorsBox : IEnumerable<IWarrior>
{ 
    int OrderSize { get; }

    IWarrior GetWarrior(int index);

    void RemoveWarrior(IWarrior warrior);
}
