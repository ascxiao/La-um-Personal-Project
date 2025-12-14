using UnityEngine;
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

        private void Start()
        {
            PrepareUI();
            //inventoryData.Initialize();
        }
        private void Awake()
        {
            playerControls = new PlayerControls();
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

        }

        private void HandleDragging(int itemIndex)
        {

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