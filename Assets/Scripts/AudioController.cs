using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource drumClickSound;
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource buttonClickSound;
    [SerializeField] private AudioSource winSound;
    [SerializeField] private AudioSource trippleWinSound;
    [SerializeField] private AudioSource denySound;


    private SpinDrum drum;

    private void Start()
    {
        //Find component on Scene with name Drum_1
        drum = GameObject.Find("Drum_1").GetComponent<SpinDrum>();
        drum.OnDrumSpinned += PlayDrumClickSound;
        PlayBackgroundMusic();
    }

    public void PlayDrumClickSound()
    {
        if (drumClickSound != null)
        {
            drumClickSound.Play();
        }
        else
        {
            Debug.LogError("Drum click sound AudioSource is not assigned.");
        }
    }

    public void PlayBackgroundMusic()
    {
        backgroundMusic.Play();
    }

    public void PlayButtonClick()
    {
        buttonClickSound.Play();
    }

    public void PlayWinSound()
    {
        winSound.Play();
    }

    public void PlayTrippleWinSound()
    {
        trippleWinSound.Play();
    }

    public void PlayDenySound()
    {
        denySound.Play();
    }
}
