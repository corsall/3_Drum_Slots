using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetHalfButton : ButtonBetModifier
{
    protected override void HandleButtonTouch()
    {
        this.betSizeHandler.ChangeBetSize((int)(this.creditsHandler.Credits / 2));
    }
}