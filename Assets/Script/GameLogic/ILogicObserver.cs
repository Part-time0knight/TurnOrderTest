public interface ILogicObserver
{
    public void Init(IGameLogic gameLogic, ILogicSubject logicSubject);
    public void LogicUpdate();
    public void KillWarrior(IWarrior warrior);
}
