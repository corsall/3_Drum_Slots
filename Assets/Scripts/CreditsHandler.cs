using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsHandler : MonoBehaviour
{
    public event Action OnCreditsChanged;

    [SerializeField] public int Credits { get; protected set; }

    private const string CreditsKey = "PlayerCredits";
    [SerializeField] private const int InitialCredits = 5;

    private void Start()
    {
        LoadCredits();
        if (OnCreditsChanged != null) OnCreditsChanged?.Invoke();
    }

    public void ChangeAmmountCredits(int amount)
    {
        Credits += amount;
        SaveCredits();
        if (OnCreditsChanged != null) OnCreditsChanged?.Invoke();
    }

    private void SaveCredits()
    {
        PlayerPrefs.SetInt(CreditsKey, Credits);
        PlayerPrefs.Save();
    }

    private void LoadCredits()
    {
        if (PlayerPrefs.HasKey(CreditsKey))
        {
            Credits = PlayerPrefs.GetInt(CreditsKey);
        }
        else
        {
            Credits = InitialCredits;
            SaveCredits();
        }
    }

    public void ResetCredits()
    {
        PlayerPrefs.DeleteKey(CreditsKey);
        Credits = 0;
        if (OnCreditsChanged != null) OnCreditsChanged?.Invoke();
    }
}
