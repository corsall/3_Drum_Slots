using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void WinEventHandler(int winAmount);

public class RouletteController : MonoBehaviour
{
    public event WinEventHandler OnWin;
    public event Action OnCreditsZero;

    [SerializeField] private List<GameObject> fruitsList;

    private List<int> winningFruits;
    private BoxCollider2D boxCollider;
    private BetSizeHandler betSizeHandler;
    private CreditsHandler creditsHandler;
    private WinConditionHandler winConditionHandler;
    private AudioController audioController;

    private GameObject drum_1;
    private GameObject drum_2;
    private GameObject drum_3;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        winConditionHandler = GetComponent<WinConditionHandler>();
        audioController = FindObjectOfType<AudioController>();

        TouchInputHandler.Instance.RegisterCollider(boxCollider, OnRouletteTouch);

        // find by name in child
        drum_1 = transform.Find("Drum_1")?.gameObject;
        drum_2 = transform.Find("Drum_2")?.gameObject;
        drum_3 = transform.Find("Drum_3")?.gameObject;

        // Check if all drums are found
        if (drum_1 == null || drum_2 == null || drum_3 == null)
        {
            Debug.LogError("One or more drums not found");
        }

        betSizeHandler = FindObjectOfType<BetSizeHandler>();
        creditsHandler = FindObjectOfType<CreditsHandler>();
    }

    void OnRouletteTouch()
    {
        if (drum_1.GetComponent<SpinDrum>().CheckIfSpinning())
        {
            audioController.PlayDenySound();
            return;
        }
        if (betSizeHandler.BetSize > 0 && creditsHandler.Credits > 0 && creditsHandler.Credits - betSizeHandler.BetSize >= 0)
        {
            creditsHandler.ChangeAmmountCredits(-betSizeHandler.BetSize);
            StartCoroutine(StartSpinAllDrumsAnimationCoroutine());
        }
        else
        {
            audioController.PlayDenySound();
            OnCreditsZero?.Invoke();
        }
    }

    IEnumerator StartSpinAllDrumsAnimationCoroutine()
    {
        SpinDrum spinDrum_1 = drum_1.GetComponent<SpinDrum>();
        SpinDrum spinDrum_2 = drum_2.GetComponent<SpinDrum>();
        SpinDrum spinDrum_3 = drum_3.GetComponent<SpinDrum>();

        Coroutine spinCoroutine_1 = StartCoroutine(spinDrum_1.AnimateDrumSpin());
        Coroutine spinCoroutine_2 = StartCoroutine(spinDrum_2.AnimateDrumSpin());
        Coroutine spinCoroutine_3 = StartCoroutine(spinDrum_3.AnimateDrumSpin());

        yield return spinCoroutine_1;
        yield return spinCoroutine_2;
        yield return spinCoroutine_3;

        int? drum_1_FruitIDResult = drum_1.GetComponent<SpinDrum>().lastFruitID;
        int? drum_2_FruitIDResult = drum_2.GetComponent<SpinDrum>().lastFruitID;
        int? drum_3_FruitIDResult = drum_3.GetComponent<SpinDrum>().lastFruitID;

        if (drum_1_FruitIDResult == null || drum_2_FruitIDResult == null || drum_3_FruitIDResult == null)
        {
            Debug.LogError("One or more drums did not return a result");
            yield break;
        }

        CheckWinInfo(drum_1_FruitIDResult.Value, drum_2_FruitIDResult.Value, drum_3_FruitIDResult.Value);
    }

    private void CheckWinInfo(int drum_1_ID, int  drum_2_ID, int drum_3_ID)
    {
        (bool isWinning, int winAmount) = winConditionHandler.GetWinInfo(drum_1_ID, drum_2_ID, drum_3_ID);

        if (isWinning)
        {
            creditsHandler.ChangeAmmountCredits(winAmount);

            audioController.PlayWinSound();

            OnWin?.Invoke(winAmount);
        }
        else
        {
            //Debug.Log("You lost");
        }
    }

    public List<GameObject> GetFruitsList() { return fruitsList; }
}
