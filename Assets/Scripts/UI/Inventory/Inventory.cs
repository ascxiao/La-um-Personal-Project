using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InventoryItem itemPrefab;
    [SerializeField] private RectTransform contentPanel;

    List<InventoryItem> itemList = new List<InventoryItem>();

    public void InitializeInventoryUI(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            InventoryItem item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            item.transform.SetParent(contentPanel, false);
            itemList.Add(item);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
