using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private Inventory inventoryUI;
    private PlayerControls playerControls;

    public int inventorySize = 6;
    private void Awake()
    {
        playerControls = new PlayerControls();
        inventoryUI.InitializeInventoryUI(inventorySize);
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
        }
        else
        {
            inventoryUI.Hide();
        }
    }
}
