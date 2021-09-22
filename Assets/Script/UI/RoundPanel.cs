using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundPanel : ItemPanel
{
    [SerializeField]
    private string text = "Раунд: ";
    private int round = 0;
    public void RoundSet(int round) => this.round = round;

    public override void UpdateText()
    {
        description.text = text + round;
    }
}
