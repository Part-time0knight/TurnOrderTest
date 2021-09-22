using UnityEngine;
public class Warrior : IWarrior
{
    public int position => _position;

    public int speed => _speed;

    public int initiative => _initiative;

    public IFraction fraction => _fraction;

    private int _position;
    private int _speed;
    private int _initiative;
    private IFraction _fraction;
    public Warrior(IFraction fraction, int speed, int initiative, int position)
    {
        _fraction = fraction;
        _position = position;
        _speed = speed;
        _initiative = initiative;
    }
    public void Death()
    {
        Debug.Log("умер мужик\nцвет" + _fraction.name + "\nскорость:" + speed + "\nпозиция:" + position + "\bининциатива:" + initiative);
    }

}
