using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InventoryItem itemPrefab;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private InventoryDescription itemDescription;
    [SerializeField] private DraggedItem draggedItem;

    List<InventoryItem> itemList = new List<InventoryItem>();

    private int currentlyDraggedItemIndex = -1;

    public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;
    public event Action<int, int> OnSwapItems;

    private void Awake()
    {
        draggedItem.Toggle(false);
    }

    public void InitializeInventoryUI(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            InventoryItem item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            item.transform.SetParent(contentPanel, false);
            itemList.Add(item);

            item.OnItemClicked += HandleItemSelection;
            item.OnItemBeginDrag += HandleBeginDrag;
            item.OnItemDroppedOn += HandleSwap;
            item.OnItemEndDrag += HandleEndDrag;
            item.OnRightMouseBtnClick += HandleShowItemActions;
        }
    }

    public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
    {
        if (itemList.Count > itemIndex)
        {
            itemList[itemIndex].SetData(itemImage, itemQuantity);
        }
    }

    private void HandleItemSelection(InventoryItem inventoryItemUI)
    {
        itemDescription.gameObject.SetActive(true);
        int index = itemList.IndexOf(inventoryItemUI);
        if (index == -1)
            return;
        OnDescriptionRequested?.Invoke(index);
    }

    private void HandleBeginDrag(InventoryItem inventoryItemUI)
    {
        int index = itemList.IndexOf(inventoryItemUI);
        if (index == -1)
            return;
        currentlyDraggedItemIndex = index;
        HandleItemSelection(inventoryItemUI);
        OnStartDragging?.Invoke(index);

    }

    public void CreateDraggedItem(Sprite sprite, int quantity)
    {
        draggedItem.Toggle(true);
        draggedItem.SetData(sprite, quantity);
    }

    private void HandleSwap(InventoryItem inventoryItemUI)
    {
        int index = itemList.IndexOf(inventoryItemUI);
        if (index == -1)
        {
            return;
        }

        OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
    }

    private void HandleEndDrag(InventoryItem inventoryItemUI)
    {
        ResetDragItem();
    }

    private void HandleShowItemActions(InventoryItem inventoryItemUI)
    {

    }
    public void Show()
    {
        gameObject.SetActive(true);
        itemDescription.ResetDescription();
        ResetSelection();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        ResetDragItem();
    }

    public void ResetDragItem()
    {
        draggedItem.Toggle(false);
        currentlyDraggedItemIndex = -1;
    }

    public void ResetSelection()
    {
        itemDescription.ResetDescription();
        DeselectAllItems();
    }

    public void DeselectAllItems()
    {
        foreach (InventoryItem item in itemList)
        {
            item.Deselect();
        }
    }
}
