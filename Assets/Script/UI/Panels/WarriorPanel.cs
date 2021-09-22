using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarriorPanel : ItemPanel
{
    [SerializeField]
    private Text _turnText;
    [SerializeField]
    private string _itemName = "Воин";
    [SerializeField]
    private string _initiative = "Инициатива";
    [SerializeField]
    private string _speed = "Скорость";

    public IWarrior Warrior => _warrior;

    public int Turn
    {
        get => _turn;
        set
        {
            _turn = value;
            UpdateText();
        }
    }

    private IWarrior _warrior;
    private int _turn;
    private Image image;

    public void WarriorSet(IWarrior warrior)
    {
        _warrior = warrior;
        float alpha = image.color.a;
        Color fractionColor = warrior.Fraction.FractionColor;
        image.color = new Color(fractionColor.r, fractionColor.g, fractionColor.b, alpha);
    }

    public override void UpdateText()
    {
        _description.text =
            $"{_itemName} {Warrior.Fraction.Name} {Warrior.Position}" +
            $":\n{_initiative} - {Warrior.Initiative} {_speed} - {Warrior.Speed}";

        _turnText.text = $"{Turn}";
    }

    private void Awake() => image = GetComponent<Image>();
}
