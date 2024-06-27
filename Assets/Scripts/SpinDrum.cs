using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinDrum : MonoBehaviour
{
    public event Action OnDrumSpinned;

    [SerializeField] private List<GameObject> fruitsList;
    private SpinDrumPreferences preferences;

    private float elapsedTimeAfterFirstAnim = 0.0f;
    private bool isSpinning = false;
    public int? lastFruitID = null;
    [SerializeField] private float wiggleMagnitude = 0.05f;
    [SerializeField] private float wiggleSpeed = 4f;

    void Awake()
    {
        RouletteController rouletteController = GetComponentInParent<RouletteController>();
        fruitsList = rouletteController.GetFruitsList();
        preferences = rouletteController.GetComponentInParent<SpinDrumPreferences>();

        CreateStartingSlot();
    }

	IEnumerator AnimateOneSpin(GameObject square)
	{
		Vector3 startPosition = square.transform.localPosition;
		Vector3 endPosition = new Vector3(startPosition.x, preferences.endY, startPosition.z);
        Vector3 lookAtPoint = new Vector3(0, 0, preferences.lookZ);

        float elapsedTime = 0;

		while (elapsedTime < preferences.oneDrumSpinDuration)
		{
			square.transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / preferences.oneDrumSpinDuration);

            square.transform.LookAt(lookAtPoint);

            elapsedTime += Time.deltaTime;
			yield return null;
		}

        Destroy(square);
    }

    public IEnumerator AnimateDrumSpin()
    {
        if (!this.isSpinning)
        {
            // Destroy all GameObject in child
            DestroyAllChildren();

            isSpinning = true;
            bool hasStartedRecursion = false;
            List<Coroutine> animations = new List<Coroutine>();

            while (elapsedTimeAfterFirstAnim < preferences.drumSpinDuration)
            {
                elapsedTimeAfterFirstAnim += Time.deltaTime;

                if (!hasStartedRecursion)
                {
                    (GameObject firstSquare, _) = CreateSlot();
                    Coroutine animateOneSpin = StartCoroutine(AnimateOneSpin(firstSquare));
                    Coroutine waitForDistanceAndCreateNextSquare = StartCoroutine(WaitForDistanceAndCreateNextSquare(firstSquare, animations));

                    animations.Add(animateOneSpin);
                    animations.Add(waitForDistanceAndCreateNextSquare);

                    hasStartedRecursion = true;
                }

                yield return null;
            }

            // Wait for all animations to finish
            foreach (var animation in animations)
            {
                yield return animation;
            }

            elapsedTimeAfterFirstAnim = 0;
            isSpinning = false;
        }
    }

    private IEnumerator WaitForDistanceAndCreateNextSquare(GameObject square, List<Coroutine> animations)
    {
        // FOR SFX
        OnDrumSpinned?.Invoke();

        (GameObject nextSquare, int lastFruitIndex) = CreateSlot();


        Coroutine animateNextSpin;

        while (((square != null) ? Mathf.Abs(square.transform.localPosition.y - preferences.startY) < preferences.distanceThreshold : true))
        {
            if (elapsedTimeAfterFirstAnim > preferences.drumSpinDuration)
            {
                lastFruitID = lastFruitIndex;
                animateNextSpin = StartCoroutine(AnimateLastSpin(nextSquare));
                yield break;
            }

            yield return null;
        }

        animateNextSpin = StartCoroutine(AnimateOneSpin(nextSquare));

        Coroutine waitForNextDistanceAndCreateNextSquare = StartCoroutine(WaitForDistanceAndCreateNextSquare(nextSquare, animations));

        animations.Add(animateNextSpin);
        animations.Add(waitForNextDistanceAndCreateNextSquare);
    }

    IEnumerator AnimateLastSpin(GameObject square)
    {
        Vector3 startPosition = square.transform.localPosition;
        Vector3 endPosition = new Vector3(startPosition.x, 0, startPosition.z);

        float elapsedTime = 0;

        while (elapsedTime < preferences.oneDrumSpinDuration)
        {
            if (square == null)
            {
                yield break;
            }
            square.transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / preferences.oneDrumSpinDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(LevitateAndWiggle(square));
    }

    IEnumerator LevitateAndWiggle(GameObject square)
    {
        Vector3 startPosition = square.transform.localPosition;
        Vector3 endPosition = new Vector3(startPosition.x, 0, startPosition.z);

        float elapsedTime = 0;

        while (true)
        {
            float levitateOffset = Mathf.Sin(elapsedTime * wiggleSpeed) * wiggleMagnitude;
            Vector3 newPosition = endPosition + Vector3.up * levitateOffset;

            if (square == null)
            {
                yield break;
            }
            square.transform.localPosition = Vector3.Lerp(startPosition, newPosition, elapsedTime / preferences.oneDrumSpinDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }


    Tuple <GameObject, int> CreateSlot()
	{
        int randomIndex = UnityEngine.Random.Range(0, fruitsList.Count);

        GameObject square = Instantiate(fruitsList[randomIndex]);
		square.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

		SpriteRenderer spriteRenderer = square.GetComponent<SpriteRenderer>();

		if (spriteRenderer == null)
		{
			spriteRenderer = square.AddComponent<SpriteRenderer>();
		}
		spriteRenderer.sortingOrder = 1;

		square.transform.parent = transform;
		square.transform.localPosition = new Vector3(0, preferences.startY, 0);

        return new Tuple<GameObject, int>(square, randomIndex);
    }

    void CreateStartingSlot()
    {
        (GameObject startingSlot, _) = CreateSlot();

        startingSlot.transform.localPosition = new Vector3(0, 0, 0);

        StartCoroutine(LevitateAndWiggle(startingSlot));
    }

    void DestroyAllChildren()
    {
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public bool CheckIfSpinning()
    {
        return isSpinning;
    }
}