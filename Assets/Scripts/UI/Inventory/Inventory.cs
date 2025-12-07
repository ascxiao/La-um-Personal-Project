using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InventoryItem itemPrefab;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private InventoryDescription itemDescription;
    [SerializeField] private DraggedItem draggedItem;

    List<InventoryItem> itemList = new List<InventoryItem>();

    public Sprite image;
    public int quantity;
    public string title, description;

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

    private void HandleItemSelection(InventoryItem obj)
    {
        itemDescription.gameObject.SetActive(true);
        itemDescription.SetDescription(image, title, description);
        itemList[0].Select();
    }

    private void HandleBeginDrag(InventoryItem obj)
    {
        draggedItem.Toggle(true);
        draggedItem.SetData(image, quantity);
    }

    private void HandleSwap(InventoryItem obj)
    {

    }

    private void HandleEndDrag(InventoryItem obj)
    {
        draggedItem.Toggle(false);
    }

    private void HandleShowItemActions(InventoryItem obj)
    {

    }
    public void Show()
    {
        gameObject.SetActive(true);
        itemDescription.ResetDescription();

        itemList[0].SetData(image, quantity);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
