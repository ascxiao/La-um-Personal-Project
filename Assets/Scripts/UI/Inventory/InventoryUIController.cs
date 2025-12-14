using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using InventoryController.UI;
using InventoryController.Model;

namespace InventoryController
{
    public class InventoryUIController : MonoBehaviour
    {
        [SerializeField] private Inventory inventoryUI;
        [SerializeField] InventorySO inventoryData;
        private PlayerControls playerControls;

        public List<InventoryItemStruct> initialItems = new List<InventoryItemStruct>();

        private void Start()
        {
            PrepareUI();
            PrepareInventoryData();
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            foreach (InventoryItemStruct item in initialItems)
            {
                if (item.IsEmpty)
                    continue;
                inventoryData.AddItem(item);
            }
        }
        private void Awake()
        {
            playerControls = new PlayerControls();
        }
        private void UpdateInventoryUI(Dictionary<int, InventoryItemStruct> inventoryState)
        {
            inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.itemName.ItemImage, item.Value.itemQuantity);
            }
        }
        private void PrepareUI()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.Size);
            this.inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            this.inventoryUI.OnSwapItems += HandleSwapItems;
            this.inventoryUI.OnStartDragging += HandleDragging;
            this.inventoryUI.OnItemActionRequested += HandleItemActionRequest;
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItemStruct inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }
            ItemSO item = inventoryItem.itemName;
            inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.Name, item.Description);
        }
        private void HandleSwapItems(int itemIndex1, int itemIndex2)
        {
            inventoryData.SwapItems(itemIndex1, itemIndex2);
        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItemStruct inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            inventoryUI.CreateDraggedItem(inventoryItem.itemName.ItemImage, inventoryItem.itemQuantity);
        }
        private void HandleItemActionRequest(int itemIndex)
        {

        }

        private void OnEnable()
        {
            playerControls.Enable();
            playerControls.Menu.Inventory.performed += Inventory;
        }

        void Inventory(InputAction.CallbackContext context)
        {
            if (inventoryUI.isActiveAndEnabled == false)
            {
                inventoryUI.Show();
                foreach (var item in inventoryData.GetCurrentInventoryState())
                {
                    inventoryUI.UpdateData(item.Key,
                    item.Value.itemName.ItemImage,
                    item.Value.itemQuantity);
                }
            }
            else
            {
                inventoryUI.Hide();
            }
        }
    }
}