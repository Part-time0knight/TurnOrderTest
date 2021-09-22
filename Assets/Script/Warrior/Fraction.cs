using UnityEngine;

public class Fraction : IFraction
{
    public Color color => _color;

    public string name => _name;

    public int priority => _priority;

    private Color _color;
    private string _name;
    private int _priority;
    public Fraction(Color color, string name, int priority)
    {
        _color = color;
        _name = name;
        _priority = priority;
    }
}
