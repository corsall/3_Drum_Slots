using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCreditsButton : MonoBehaviour
{
    CreditsHandler creditsHandler;
    void Start()
    {
        creditsHandler = FindObjectOfType<CreditsHandler>();
    }

    public void OnClick()
    {
        creditsHandler.ChangeAmmountCredits(1);
    }
}
