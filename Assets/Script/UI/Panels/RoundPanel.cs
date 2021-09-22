using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundPanel : ItemPanel
{
    [SerializeField]
    private string _text = "Раунд:";
    private int _round = 0;

    public void RoundSet(int round) => _round = round;

    public override void UpdateText() => _description.text = $"{_text} {_round}";
}
