public interface IWarrior
{
    int Position { get; }
    int Speed { get; }
    int Initiative { get; }

    IFraction Fraction { get; }

    void Death();
}
