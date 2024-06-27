using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinConditionHandler : MonoBehaviour
{
    BetSizeHandler betSizeHandler;
    AudioController audioController;

    private void Awake()
    {
        betSizeHandler = FindObjectOfType<BetSizeHandler>();
        audioController = FindObjectOfType<AudioController>();
    }

    private int d_1_result;
    private int d_2_result;
    private int d_3_result;


    public (bool isWinning, int winAmmount) GetWinInfo(int d_1_result, int d_2_result, int d_3_result)
    {
        this.d_1_result = d_1_result;
        this.d_2_result = d_2_result;
        this.d_3_result = d_3_result;

        // Chain of checks

        // X2
        if (CheckSameInRow(d_1_result, d_2_result, d_3_result, out var winAmount))
        {
            audioController.PlayTrippleWinSound();
            return (true, winAmount);
        }

        // +2
        if (CheckIfHasSuperFruit(d_1_result, d_2_result, d_3_result, out winAmount))
        {
            return (true, winAmount);
        }

        // +10
        if (CheckIfHasTwoFruitID(d_1_result, d_2_result, d_3_result, out winAmount))
        {
            return (true, winAmount);
        }


        return (false, 0);
    }

    private bool CheckSameInRow(int d_1_result, int d_2_result, int d_3_result, out int winAmount)
    {
        if (d_1_result == d_2_result && d_2_result == d_3_result)
        {
            winAmount = betSizeHandler.BetSize * 2;
            return true;
        }

        winAmount = 0;
        return false;
    }

    private bool CheckIfHasSuperFruit(int d_1_result, int d_2_result, int d_3_result, out int winAmount)
    {
        winAmount = 0;

        if (d_1_result == 1)
        {
            winAmount += 2;
        }
        if (d_2_result == 1)
        {
            winAmount += 2;
        }
        if (d_3_result == 1)
        {
            winAmount += 2;
        }

        if (winAmount > 0)
        {
            return true;
        }

        return false;
    }

    private bool CheckIfHasTwoFruitID(int d_1_result, int d_2_result, int d_3_result, out int winAmount)
    {
        int count = 0;

        if (d_1_result == 3) count++;
        if (d_2_result == 3) count++;
        if (d_3_result == 3) count++;

        if (count >= 2)
        {
            winAmount = betSizeHandler.BetSize + 5;
            return true;
        }

        winAmount = 0;
        return false;
    }
}
