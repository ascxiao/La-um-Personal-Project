using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace InventoryController.Model
{
    [CreateAssetMenu(fileName = "InventorySO", menuName = "Scriptable Objects/InventorySO")]
    public class InventorySO : ScriptableObject
    {
        [field: SerializeField]
        public List<InventoryItemStruct> inventoryItems;

        [field: SerializeField]
        public int Size { get; set; } = 10;

        public void Initialize()
        {
            inventoryItems = new List<InventoryItemStruct>();
            for (int i = 0; i < Size; i++)
            {
                inventoryItems.Add(InventoryItemStruct.GetEmptyItem());
            }
        }

        public void AddItem(ItemSO item, int quantity)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                {
                    inventoryItems[i] = new InventoryItemStruct
                    {
                        itemName = item,
                        itemQuantity = quantity
                    };
                }
            }
        }

        public Dictionary<int, InventoryItemStruct> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItemStruct> returnValue = new Dictionary<int, InventoryItemStruct>();

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                    continue;
                returnValue[i] = inventoryItems[i];
            }
            return returnValue;
        }

        public InventoryItemStruct GetItemAt(int itemIndex)
        {
            return inventoryItems[itemIndex];
        }
    }

    [Serializable]
    public struct InventoryItemStruct
    {
        public int itemQuantity;
        public ItemSO itemName;
        public bool IsEmpty => itemName == null;

        public InventoryItemStruct ChangeQuantity(int newQuantity)
        {
            return new InventoryItemStruct
            {
                itemName = this.itemName,
                itemQuantity = newQuantity,
            };
        }
        public static InventoryItemStruct GetEmptyItem() => new InventoryItemStruct
        {
            itemName = null,
            itemQuantity = 0,
        };
    }
}