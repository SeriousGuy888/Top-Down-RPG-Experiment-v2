using System;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;

public class InventoryUI : MonoBehaviour {
  public Inventory inventory;
  public Equipment equipment;

  public InventorySlot slotPrefab;

  public Transform inventorySlotsContainer;
  public InventoryDescription descriptionPanel;
  public MouseFollower mouseFollower;

  private InventorySlot[] inventorySlots;
  private int currentDraggedItemIndex = -1;

  public event Action<int>
    OnDescriptionRequested,
    OnItemActionsRequested,
    OnDragStart;
  public event Action<int, int>
    OnSwapItems;


  private void Awake() {
    Hide();
    mouseFollower.Toggle(false);
    descriptionPanel.ResetDescription();

    // inventory.onItemChangedCallback += UpdateUI;
    // equipment.onItemChangedCallback += UpdateUI;
  }

  public void Show() {
    gameObject.SetActive(true);
    ResetSelection();

    var inventoryState = inventory.GetInventoryState();
    foreach(var entry in inventoryState) {
      int slotIndex = entry.Key;
      int quantity = entry.Value.quantity;
      Item item = entry.Value.item;

      inventorySlots[slotIndex].SetData(item.icon, quantity);
    }
  }
  public void Hide() {
    gameObject.SetActive(false);
    ResetDraggedItem();
  }

  public void InitInventoryUI(int inventorySize) {
    inventorySlots = new InventorySlot[inventorySize];
    for (int i = 0; i < inventorySize; i++) {
      var newSlot = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity);
      newSlot.transform.SetParent(inventorySlotsContainer);
      inventorySlots[i] = newSlot;

      newSlot.OnItemClicked += HandleItemSelect;
      newSlot.OnItemRightClicked += HandleItemShowActions;
      newSlot.OnItemDragStart += HandleDragStart;
      newSlot.OnItemDragEnd += HandleDragEnd;
      newSlot.OnItemDroppedOn += HandleSwap;
    }
  }


  public void UpdateSlotData(int index, Sprite icon, int quantity) {
    if (inventorySlots.Length > index) {
      inventorySlots[index].SetData(icon, quantity);
    }
  }


  private void HandleItemSelect(InventorySlot slot) {
    int index = Array.IndexOf(inventorySlots, slot);
    if (index == -1)
      return;

    OnDescriptionRequested?.Invoke(index);
  }

  private void HandleItemShowActions(InventorySlot slot) {

  }

  private void HandleDragStart(InventorySlot slot) {
    int index = Array.IndexOf(inventorySlots, slot);
    if (index == -1)
      return;
    currentDraggedItemIndex = index;

    HandleItemSelect(slot); // select the slot that is being dragged
    OnDragStart?.Invoke(index);
  }

  private void HandleDragEnd(InventorySlot slot) {
    ResetDraggedItem();
  }

  private void HandleSwap(InventorySlot slot) {
    int swappingIndex = Array.IndexOf(inventorySlots, slot);
    if (swappingIndex == -1)
      return;

    OnSwapItems?.Invoke(currentDraggedItemIndex, swappingIndex);
  }


  private void CreateDraggedItem(Sprite sprite, int quantity) {
    mouseFollower.Toggle(true);
    mouseFollower.SetData(sprite, quantity);
  }

  private void ResetDraggedItem() {
    mouseFollower.Toggle(false);
    currentDraggedItemIndex = -1;
  }

  private void ResetSelection() {
    descriptionPanel.ResetDescription();
    foreach (var slot in inventorySlots) {
      slot.Deselect();
    }
  }


  private void UpdateUI() {
    // for (int i = 0; i < inventorySlots.Length; i++) {
    //   if (i < inventory.items.Count)
    //     inventorySlots[i].SetItem(inventory.items[i]);
    //   else
    //     inventorySlots[i].ClearSlot();
    // }

    // for (int i = 0; i < equipSlots.Length; i++) {
    //   Item item = equipment.items[i];
    //   if(item != null)
    //     equipSlots[i].SetItem(item);
    //   else
    //     equipSlots[i].ClearSlot();
    // }
  }
}
