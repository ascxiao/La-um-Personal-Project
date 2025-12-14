using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InventoryController.UI;

namespace InventoryController.Model
{
    [CreateAssetMenu(fileName = "InventorySO", menuName = "Scriptable Objects/InventorySO")]
    public class InventorySO : ScriptableObject
    {
        [field: SerializeField]
        public List<InventoryItemStruct> inventoryItems;

        [field: SerializeField]
        public int Size { get; set; } = 10;
        public event Action<Dictionary<int, InventoryItemStruct>> OnInventoryUpdated;

        public void Initialize()
        {
            inventoryItems = new List<InventoryItemStruct>();
            for (int i = 0; i < Size; i++)
            {
                inventoryItems.Add(InventoryItemStruct.GetEmptyItem());
            }
        }

        public int AddItem(ItemSO item, int quantity)
        {
            if (!item.isStackable)
            {
                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    while (quanitity > 0 && !IsInventoryFull())
                    {
                        quantity -= AddNonStackableItem(item, 1);
                    }
                    InformAboutChange();
                    return quantity;
                }
            }
            quantity = AddItemToFirstFreeSlot(item, quantity);
            InformAboutChange();
            return quantity;
        }

        private int AddNonStackableItem(ItemSO item, int quantity)
        {
            InventoryItemStruct newItem = new InventoryItemStruct
            {
                itemName = item,
                itemQuantity = quantity
            };

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                {
                    inventoryItems[i] = newItem;
                    return quantity;
                }
            }
            return 0;
        }
        private bool IsInventoryFull() => !inventoryItems.Where(item => item.IsEmpty).Any();
        private int AddItemToFirstFreeSlot(ItemSO item, int quantity)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                    continue;
                if (inventoryItems[i].itemName.ID == item.ID)
                {
                    int amountPossibleToTake = inventoryItems[i].itemName.MaxStackSize - inventoryItems[i].itemQuantity;

                    if (quantity > amountPossibleToTake)
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].itemName.MaxStackSize);
                        quantity -= amountPossibleToTake;
                    }
                    else
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].itemQuantity + quantity);
                        InformAboutChange();
                        return 0;
                    }
                }
            }
            while (quantity > 0 && !IsInventoryFull())
            {
                int newQuantity = MathF.Clamp(quantity, 0, item.MaxStackSize);
                quantity -= newQuantity;
                AddItemToFirstFreeSlot(item, newQuantity);
            }
            return quantity;
        }

        public void AddItem(InventoryItemStruct item)
        {
            AddItem(item.itemName, item.itemQuantity);
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

        public void SwapItems(int item1Index, int item2Index)
        {
            InventoryItemStruct item1 = inventoryItems[item1Index];
            inventoryItems[item1Index] = inventoryItems[item2Index];
            inventoryItems[item2Index] = item1;
            InformAboutChange();
        }

        private void InformAboutChange()
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
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