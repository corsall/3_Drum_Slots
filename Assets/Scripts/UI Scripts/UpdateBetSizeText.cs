using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateBetSizeText : MonoBehaviour
{
    BetSizeHandler betSizeHandler;
    TextMeshProUGUI textMeshProUGUI;

    private void Awake()
    {
        betSizeHandler = FindObjectOfType<BetSizeHandler>();

        textMeshProUGUI = this.GetComponent<TextMeshProUGUI>();

        if (betSizeHandler != null)
        {
            betSizeHandler.OnBetSizeChanged += UpdateBetSizeUI;
        }
    }

    private void UpdateBetSizeUI()
    {
        textMeshProUGUI.text = "BET SIZE: " + betSizeHandler.BetSize;
    }
}
