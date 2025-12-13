using UnityEngine;
using System;

[CreateAssetMenu(fileName = "InventorySO", menuName = "Scriptable Objects/InventorySO")]
public class InventorySO : ScriptableObject
{
    [field: SerializeField]
    public List<InventoryItem> inventoryItems;

    [field: SerializeField]
    public int Size { get; set; } = 10;

    public void Initialize()
    {
        inventoryItems = new List<InventoryItem>();
        for (int i = 0; i < Size; i++)
        {
            inventoryItems.Add(InventoryItem.GetEmptyItem());
        }
    }

    public void AddItem(ItemSO item, int quantity)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].IsEmpty)
            {
                inventoryItems[i] = new InventoryItem
                {
                    itemName = item,
                    itemQuantity = quantity
                };
            }
        }
    }
}

[Serializable]
public struct InventoryItem
{
    public int itemQuantity;
    public ItemSO itemName;
    public bool IsEmpty => iteml == null;

    public InventoryItem ChangeQuantity(InventoryItem newQuantity)
    {
        return new InventoryItem
        {
            item = this.item,
            quantity = newQuantity,
        };
    }
    public static InventoryItem GetEmptyItem() => new InventoryItem
    {
        item = null,
        quantity = 0,
    };
}