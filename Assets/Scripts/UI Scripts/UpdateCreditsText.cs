using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateCreditsText : MonoBehaviour
{
    CreditsHandler creditsHandler;
    TextMeshProUGUI textMeshProUGUI;

    private void Awake()
    {
        creditsHandler = FindObjectOfType<CreditsHandler>();

        textMeshProUGUI = this.GetComponent<TextMeshProUGUI>();

        if (creditsHandler != null)
        {
            creditsHandler.OnCreditsChanged += UpdateCreditsUI;
        }
    }

    private void UpdateCreditsUI()
    {
        textMeshProUGUI.text = "CREDITS: " + creditsHandler.Credits;
    }
}
