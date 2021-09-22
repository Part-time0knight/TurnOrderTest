using UnityEngine;
public class Warrior : IWarrior
{
    public int Identifier => _identifier;
    public int Hash => _hash;
    public int Position => _position;
    public int Speed => _speed;
    public int Initiative => _initiative;

    public IFraction Fraction => _fraction;

    private int _identifier;
    private int _hash;
    private int _position;
    private int _speed;
    private int _initiative;
    private IFraction _fraction;

    public Warrior(IFraction fraction, int speed, int initiative, int position, int identifier)
    {
        System.Random random = new System.Random();
        _fraction = fraction;
        _position = position;
        _speed = speed;
        _initiative = initiative;
        _identifier = identifier;
        _hash = _identifier * random.Next();
    }

    public void Death()
        => Debug.Log($"умер мужик\nцвет: { _fraction.Name }\nскорость: {Speed}" +
            $"\nпозиция: {Position}\bининциатива:{Initiative}");

    public override int GetHashCode()
    {
        return _identifier.GetHashCode();
    }

    private bool Equals(Warrior warrior)
    {
        return warrior != null &&
            warrior.Identifier == _identifier;
    }
}
