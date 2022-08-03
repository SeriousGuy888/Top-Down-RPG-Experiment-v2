using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour {
  public static Inventory Instance;
  private void Awake() {
    Instance = this;
  }

  private InputMaster controls;

  public InventoryData inventoryData; // model
  public InventoryUI inventoryUI; // view

  public List<ItemAssignedSlot>[] slotsAccept;




  public int mainSlotCount = 12;
  public int equipmentSlotCount = 6;

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
    int totalSlotCount = mainSlotCount + equipmentSlotCount;
    slotsAccept = new List<ItemAssignedSlot>[totalSlotCount];

    inventoryData.Init(totalSlotCount);
    inventoryData.OnInventoryUpdate += UpdateUI;
  }

  private void PrepareUI() {
    inventoryUI.InitInventoryUI();
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


  private string PrepareDescription(InventoryItem invItem) {
    StringBuilder sb = new();
    sb.Append(invItem.item.description);
    sb.AppendLine();

    for (int i = 0; i < invItem.properties.Count; i++) {
      var currProp = invItem.properties[i];
      sb.Append($"{currProp.property.Name}: {currProp.value}/{invItem.item.defaultPropertiesList[i].value}");
      sb.AppendLine();
    }

    return sb.ToString();
  }

  private void HandleDescriptionRequest(int index) {
    var invItem = inventoryData.GetItem(index);
    if (invItem.IsEmpty)
      return;

    var item = invItem.item;
    var description = PrepareDescription(invItem);

    inventoryUI.SetDescription(item.icon, item.name, description);
  }

  private void HandleItemSwap(int indexA, int indexB) {
    var itemA = inventoryData.GetItem(indexA).item;
    var itemB = inventoryData.GetItem(indexB).item;

    var slotAAccepts = slotsAccept[indexA];
    var slotBAccepts = slotsAccept[indexB];

    if (itemA != null && !itemA.IsValidSlot(slotBAccepts))
      return;
    if (itemB != null && !itemB.IsValidSlot(slotAAccepts))
      return;

    inventoryData.SwapItems(indexA, indexB);
    inventoryUI.SelectSlot(indexB);
  }

  private void HandleDrag(int index) {
    var inventoryItem = inventoryData.GetItem(index);
    if (inventoryItem.IsEmpty)
      return;

    inventoryUI.CreateDraggedItem(inventoryItem.item.icon, inventoryItem.quantity);
  }

  private void HandleItemActionsRequest(int index) {
    var inventoryItem = inventoryData.GetItem(index);
    if (inventoryItem.IsEmpty)
      return;

    var action = inventoryItem.item as IItemAction;
    if (action != null)
      action.Perform(GameManager.Instance.player.gameObject, null);

    var destroyable = inventoryItem.item as IDestroyableItem;
    Debug.Log(destroyable);
    if (destroyable != null)
      inventoryData.Remove(index, 1);
  }
}