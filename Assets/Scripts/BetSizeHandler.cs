using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetSizeHandler : MonoBehaviour
{
    public event Action OnBetSizeChanged;

    [SerializeField] public int BetSize { get; protected set; }

    [SerializeField] private const int InitialBetSize = 1;

    private void Start()
    {
        ChangeBetSize(InitialBetSize);
    }

    public void ChangeBetSize(int amount)
    {
        BetSize = amount;
        if (OnBetSizeChanged != null) OnBetSizeChanged?.Invoke();
    }

    public void AddBetSize(int amount)
    {
        BetSize += amount;
        if (OnBetSizeChanged != null) OnBetSizeChanged?.Invoke();
    }
}
