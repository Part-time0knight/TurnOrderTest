public interface ILogicSubject
{
    void AddObserver(ILogicObserver logicObserver);

    void removeObserver(ILogicObserver logicObserver);
}
