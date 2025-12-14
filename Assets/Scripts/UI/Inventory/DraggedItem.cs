using UnityEngine;
using InventoryController.UI;
using System.Collections.Generic;
using System.Collections;

public class DraggedItem : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private InventoryItem item;
    [SerializeField] private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>();
        item = GetComponentInChildren<InventoryItem>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void SetData(Sprite sprite, int quantity)
    {
        item.SetData(sprite, quantity);
    }

    void Update()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            Input.mousePosition,
            canvas.worldCamera,
            out position);
        transform.position = canvas.transform.TransformPoint(position);
    }

    public void Toggle(bool val)
    {
        gameObject.SetActive(val);
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = !val; // Disable raycasts when dragging
        }
    }
}
