using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseButton : ButtonBetModifier
{
    protected override void HandleButtonTouch()
    {
        if (this.betSizeHandler.BetSize + 1 <= this.creditsHandler.Credits)
        {
            this.betSizeHandler.AddBetSize(1);
        }
    }
}
