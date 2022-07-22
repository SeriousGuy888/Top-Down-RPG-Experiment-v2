using UnityEngine;

public class Inventory : MonoBehaviour {
  public static Inventory Instance;
  private void Awake() {
    Instance = this;
  }

  private InputMaster controls;

  public InventoryData inventoryData; // model
  public InventoryUI inventoryUI; // view

  public int inventorySize = 12;

  private void Start() {
    inventoryData.Init(inventorySize);
    PrepareUI();

    controls = GameManager.Instance.controls;
    controls.Player.ToggleInventory.performed += _ => {
      if (inventoryUI.gameObject.activeSelf) {
        inventoryUI.Hide();
      } else {
        inventoryUI.Show();

        var inventoryState = inventoryData.GetInventoryState();
        foreach (var entry in inventoryState) {
          int slotIndex = entry.Key;
          int quantity = entry.Value.quantity;
          Item item = entry.Value.item;

          inventoryUI.SetSlotData(slotIndex, item.icon, quantity);
        }
      }
    };
  }

  private void PrepareUI() {
    inventoryUI.InitInventoryUI(inventorySize);
    inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
    inventoryUI.OnSwapItems += HandleItemSwap;
    inventoryUI.OnDragStart += HandleDrag;
    inventoryUI.OnItemActionsRequested += HandleItemActionsRequest;
  }

  private void HandleDescriptionRequest(int index) {
    var inventoryItem = inventoryData.GetItem(index);
    if(inventoryItem.IsEmpty)
      return;

    var item = inventoryItem.item;
    inventoryUI.SetDescription(item.icon, item.name, item.description);
  }
  private void HandleItemSwap(int indexA, int indexB) { }
  private void HandleDrag(int index) { }
  private void HandleItemActionsRequest(int index) { }
}