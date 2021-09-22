using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarriorPanel : ItemPanel
{
    [SerializeField]
    private Text turnText;
    [SerializeField]
    private string itemName = "Воин";
    [SerializeField]
    private string initiative = "Инициатива";
    [SerializeField]
    private string speed = "Скорость";

    public IWarrior warrior => _warrior;
    public int turn
    {
        get
        {
            return _turn;
        }
        set
        {
            _turn = value;
            UpdateText();
        }
    }

    private IWarrior _warrior;
    private int _turn;
    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void WarriorSet(IWarrior warrior)
    {
        _warrior = warrior;
        float alpha = image.color.a;
        Color fractionColor = warrior.fraction.color;
        image.color = new Color(fractionColor.r, fractionColor.g, fractionColor.b, alpha);
    }
    public override void UpdateText()
    {
        description.text = itemName + " " + warrior.fraction.name + warrior.position
            + ":\n" + initiative + " - " + warrior.initiative + " " + speed + " - "
            + warrior.speed;
        turnText.text = "" + turn;
    }
}
