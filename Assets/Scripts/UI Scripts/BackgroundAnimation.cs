using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAnimation : MonoBehaviour
{
    [SerializeField] private List<GameObject> fruitsList;
    [SerializeField] private Vector2Int gridSize = new Vector2Int(8, 8);
    [SerializeField] private float spacing = 1.0f;
    [SerializeField] private float moveSpeed = 0.1f;
    [SerializeField] private float spawnInterval = 1.0f;
    [SerializeField] private float lifetime = 10.0f;
    [SerializeField] private Color fruitColor = new Color(0.5f, 0.5f, 0.5f, 0.7f);

    void Start()
    {
        StartCoroutine(SpawnFruitsContinuously());
    }

    IEnumerator SpawnFruitsContinuously()
    {
        while (true)
        {
            PlaceFruitsInDiamond();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void PlaceFruitsInDiamond()
    {
        int centerX = gridSize.x / 2;
        int centerY = gridSize.y / 2;

        for (int x = 0; x <= gridSize.x; x++)
        {
            for (int y = 0; y <= gridSize.y; y++)
            {
                // Calculate the Manhattan distance from the center
                int distance = Mathf.Abs(x - centerX) + Mathf.Abs(y - centerY);

                // If the distance is within the bounds, place the fruit
                if (distance <= centerX)
                {
                    Vector3 spawnPosition = new Vector3(x * spacing, y * spacing, 0);

                    int randomIndex = UnityEngine.Random.Range(0, fruitsList.Count);

                    GameObject fruit = Instantiate(fruitsList[randomIndex], spawnPosition, Quaternion.identity);
                    fruit.transform.SetParent(transform);

                    // Change color and opacity
                    Renderer fruitRenderer = fruit.GetComponent<Renderer>();
                    if (fruitRenderer != null)
                    {
                        fruitRenderer.material.color = fruitColor;
                    }

                    StartCoroutine(DestroyFruitAfterTime(fruit, lifetime));
                }
            }
        }
    }

    void Update()
    {
        foreach (Transform child in transform)
        {
            child.position += new Vector3(-moveSpeed, -moveSpeed, 0) * Time.deltaTime;
        }
    }

    IEnumerator DestroyFruitAfterTime(GameObject fruit, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(fruit);
    }
}
