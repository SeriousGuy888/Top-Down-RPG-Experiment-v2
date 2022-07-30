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
  }

  public void Show() {
    gameObject.SetActive(true);
    ResetSelection();
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


  public void SetSlotData(int index, Sprite icon, int quantity) {
    if (inventorySlots.Length > index) {
      inventorySlots[index].SetData(icon, quantity);
    }
  }
  public void SetDescription(Sprite icon, string title, string description) {
    descriptionPanel.SetDescription(icon, title, description);
  }


  private void HandleItemSelect(InventorySlot slot) {
    int index = Array.IndexOf(inventorySlots, slot);
    if (index == -1)
      return;

    ResetSelection();
    slot.Select();

    OnDescriptionRequested?.Invoke(index);
  }

  private void HandleItemShowActions(InventorySlot slot) {
    int index = Array.IndexOf(inventorySlots, slot);
    if (index == -1)
      return;
    OnItemActionsRequested?.Invoke(index);
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

    // swappingIndex can be -1 if the slot dragged TO doesn't exist
    // currentDraggedItemIndex can be -1 if the slot dragged FROM is empty
    if (swappingIndex == -1 || currentDraggedItemIndex == -1)
      return;

    OnSwapItems?.Invoke(currentDraggedItemIndex, swappingIndex);
    HandleItemSelect(slot); // select the new slot of the dragged item
  }


  public void CreateDraggedItem(Sprite sprite, int quantity) {
    mouseFollower.Toggle(true);
    mouseFollower.SetData(sprite, quantity);
  }

  public void ResetDraggedItem() {
    mouseFollower.Toggle(false);
    currentDraggedItemIndex = -1;
  }

  private void ResetSelection() {
    descriptionPanel.ResetDescription();
    foreach (var slot in inventorySlots) {
      slot.Deselect();
    }
  }

  public void ResetSlots() {
    ResetSelection();
    foreach (var slot in inventorySlots) {
      slot.ResetData();
    }
  }
}
