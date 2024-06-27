using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecreaseButton : ButtonBetModifier
{
    protected override void HandleButtonTouch()
    {
        if (this.betSizeHandler.BetSize - 1 >= 1)
        {
            this.betSizeHandler.AddBetSize(-1);
        }
    }
}
