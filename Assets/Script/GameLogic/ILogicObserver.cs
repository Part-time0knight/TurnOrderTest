public interface ILogicObserver
{
    void Init(IGameLogic gameLogic);

    void LogicUpdate();

    void KillWarrior(IWarrior warrior);
}
