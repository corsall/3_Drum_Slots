using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetMaxButton : ButtonBetModifier
{
    protected override void HandleButtonTouch()
    {
        this.betSizeHandler.ChangeBetSize(this.creditsHandler.Credits);
    }
}

