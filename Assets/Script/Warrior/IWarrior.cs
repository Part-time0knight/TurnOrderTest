public interface IWarrior
{
    public int position { get; }
    public int speed { get; }
    public int initiative { get; }
    public IFraction fraction { get; }
    public void Death();
}
