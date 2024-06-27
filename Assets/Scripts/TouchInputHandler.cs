using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputHandler : MonoBehaviour
{
    public delegate void ButtonTouchAction();

    private static TouchInputHandler instance;

    public static TouchInputHandler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TouchInputHandler>();
                if (instance == null)
                {
                    Debug.LogError("TouchInputHandler instance not found in the scene. :(");
                }
            }
            return instance;
        }
    }

    private Dictionary<BoxCollider2D, ButtonTouchAction> colliderActions = new Dictionary<BoxCollider2D, ButtonTouchAction>();

    public void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touchPosition = touch.position;

                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);

                foreach (var entry in colliderActions)
                {
                    if (entry.Key != null && entry.Key.bounds.Contains(worldPosition))
                    {
                        entry.Value?.Invoke();
                        break;
                    }
                }
            }
        }
    }

    public void RegisterCollider(BoxCollider2D collider, ButtonTouchAction action)
    {
        if (!colliderActions.ContainsKey(collider))
        {
            colliderActions.Add(collider, action);
        }
    }

    public void UnregisterCollider(BoxCollider2D collider)
    {
        if (colliderActions.ContainsKey(collider))
        {
            colliderActions.Remove(collider);
        }
    }
}
