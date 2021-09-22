public interface ILogicSubject
{
    public void AddObserver(ILogicObserver logicObserver);
    public void removeObserver(ILogicObserver logicObserver);
}
