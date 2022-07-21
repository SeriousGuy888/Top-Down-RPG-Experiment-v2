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

  private void Awake() {
    Hide();
    mouseFollower.Toggle(false);
    descriptionPanel.ResetDescription();

    // inventory.onItemChangedCallback += UpdateUI;
    // equipment.onItemChangedCallback += UpdateUI;
  }

  public void Show() {
    gameObject.SetActive(true);
    descriptionPanel.ResetDescription();

    for(int i = 0; i < inventory.items.Length; i++) {
      Item item = inventory.items[i];
      if(item == null)
        continue;
        
      inventorySlots[i].SetData(item.icon, 1);
    }
  }
  public void Hide() => gameObject.SetActive(false);

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


  private void HandleItemSelect(InventorySlot slot) {
    Debug.Log(slot.name);
  }

  private void HandleItemShowActions(InventorySlot slot) {

  }

  private void HandleDragStart(InventorySlot slot) {
    int index = Array.IndexOf(inventorySlots, slot);
    if(index == -1)
      return;
    currentDraggedItemIndex = index;

    mouseFollower.Toggle(true);
    mouseFollower.SetData(inventory.items[index].icon, 1);
  }

  private void HandleDragEnd(InventorySlot slot) {
    mouseFollower.Toggle(false);
    currentDraggedItemIndex = -1;
  }

  private void HandleSwap(InventorySlot slot) {
    int swappingIndex = Array.IndexOf(inventorySlots, slot);
    if (swappingIndex == -1) {
      mouseFollower.Toggle(false);
      currentDraggedItemIndex = -1;
      return;
    }

    inventorySlots[currentDraggedItemIndex].SetData(inventory.items[swappingIndex].icon, 1);
    inventorySlots[swappingIndex].SetData(inventory.items[currentDraggedItemIndex].icon, 1);
    
    mouseFollower.Toggle(false);
    currentDraggedItemIndex = -1;
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
