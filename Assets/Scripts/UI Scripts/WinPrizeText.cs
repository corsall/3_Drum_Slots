using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinPrizeText : MonoBehaviour
{
    private RouletteController rouletteController;
    private TextMeshProUGUI prizeText;
    [SerializeField] private float waitSeconds = 1f;
    [SerializeField] private float wiggleDuration = 1f;
    [SerializeField] private float wiggleMagnitude = 10f;

    void Start()
    {
        prizeText = GetComponent<TextMeshProUGUI>();
        prizeText.gameObject.SetActive(false);
        rouletteController = FindObjectOfType<RouletteController>();
        rouletteController.OnWin += ShowWinPrize;
    }

    private void ShowWinPrize(int winAmount)
    {
        prizeText.gameObject.SetActive(true);
        StartCoroutine(ShowWinPrizeCoroutine(winAmount));
    }

    private IEnumerator ShowWinPrizeCoroutine(int winAmount)
    {
        prizeText.text = "YOUR PRIZE " + winAmount.ToString();
        StartCoroutine(WiggleText());

        yield return new WaitForSeconds(waitSeconds);

        prizeText.gameObject.SetActive(false);
    }

    private IEnumerator WiggleText()
    {
        float elapsed = 0f;
        Quaternion originalRotation = prizeText.transform.rotation;

        while (elapsed < wiggleDuration)
        {
            float angle = Mathf.Sin(elapsed * Mathf.PI * 4) * wiggleMagnitude;
            prizeText.transform.rotation = originalRotation * Quaternion.Euler(0, 0, angle);
            elapsed += Time.deltaTime;
            yield return null;
        }

        prizeText.transform.rotation = originalRotation;
    }
}
