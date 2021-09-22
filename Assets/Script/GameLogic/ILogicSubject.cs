public interface ILogicSubject
{
    void AddObserver(ILogicObserver logicObserver);

    void RemoveObserver(ILogicObserver logicObserver);
}
