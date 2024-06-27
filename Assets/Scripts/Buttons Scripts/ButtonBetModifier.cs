using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ButtonBetModifier : MonoBehaviour
{
    private AudioController audioController;

    private BoxCollider2D boxCollider;
    protected BetSizeHandler betSizeHandler;
    protected CreditsHandler creditsHandler;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        betSizeHandler = FindObjectOfType<BetSizeHandler>();
        creditsHandler = FindObjectOfType<CreditsHandler>();
        audioController = FindObjectOfType<AudioController>();
    }

    protected virtual void Start()
    {
        TouchInputHandler.Instance.RegisterCollider(boxCollider, OnButtonTouch);
    }

    private void OnButtonTouch()
    {
        CommonHandleButtonTouch();
        HandleButtonTouch();
        StartCoroutine(ChangeButtonColorOnClick());
    }

    private void CommonHandleButtonTouch()
    {
        audioController.PlayButtonClick();
    }

    private IEnumerator ChangeButtonColorOnClick()
    {
        Color currentColor = spriteRenderer.color;

        float darkenFactor = 0.8f;
        Color darkerColor = currentColor * darkenFactor;

        spriteRenderer.color = darkerColor;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = currentColor;
    }

    protected abstract void HandleButtonTouch();
}
