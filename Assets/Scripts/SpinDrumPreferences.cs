using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinDrumPreferences : MonoBehaviour
{
    [SerializeField] public float oneDrumSpinDuration = 0.6f;
    [SerializeField] public float drumSpinDuration = 10.0f;
    [SerializeField] public float startY = 1f;
    [SerializeField] public float endY = -1f;
    [SerializeField] public float distanceThreshold = 1.05f;
    [SerializeField] public float lookZ = 5f;
}
