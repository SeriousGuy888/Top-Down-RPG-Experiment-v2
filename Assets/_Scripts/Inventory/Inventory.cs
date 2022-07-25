using System.Collections.Generic;
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
    PrepareData();
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

  private void PrepareData() {
    inventoryData.Init(inventorySize);
    inventoryData.OnInventoryUpdate += UpdateUI;
  }

  private void PrepareUI() {
    inventoryUI.InitInventoryUI(inventorySize);
    inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
    inventoryUI.OnSwapItems += HandleItemSwap;
    inventoryUI.OnDragStart += HandleDrag;
    inventoryUI.OnItemActionsRequested += HandleItemActionsRequest;
  }

  private void UpdateUI(Dictionary<int, InventoryItem> inventoryState) {
    inventoryUI.ResetSlots();
    foreach (var item in inventoryState) {
      inventoryUI.SetSlotData(item.Key, item.Value.item.icon, item.Value.quantity);
    }
  }


  private void HandleDescriptionRequest(int index) {
    var inventoryItem = inventoryData.GetItem(index);
    if(inventoryItem.IsEmpty)
      return;

    var item = inventoryItem.item;
    inventoryUI.SetDescription(item.icon, item.name, item.description);
  }
  
  private void HandleItemSwap(int indexA, int indexB) {
    inventoryData.SwapItems(indexA, indexB);
  }

  private void HandleDrag(int index) {
    var inventoryItem = inventoryData.GetItem(index);
    if(inventoryItem.IsEmpty)
      return;

    inventoryUI.CreateDraggedItem(inventoryItem.item.icon, inventoryItem.quantity);
  }
  
  private void HandleItemActionsRequest(int index) {
    var inventoryItem = inventoryData.GetItem(index);
    if (inventoryItem.IsEmpty)
      return;

    var action = inventoryItem.item as IItemAction;
    if(action != null)
      action.Perform(GameManager.Instance.player.gameObject);

    var destroyable = inventoryItem.item as IDestroyableItem;
    Debug.Log(destroyable);
    if(destroyable != null)
      inventoryData.Remove(index, 1);
  }
}