using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetOneButton : ButtonBetModifier
{
    protected override void HandleButtonTouch()
    {
        this.betSizeHandler.ChangeBetSize(1);
    }
}
