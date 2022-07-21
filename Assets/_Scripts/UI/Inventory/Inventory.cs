using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
  public static Inventory Instance;
  private void Awake() {
    Instance = this;
  }

  private InputMaster controls;

  public delegate void OnItemChanged();
  public OnItemChanged onItemChangedCallback;

  public InventoryUI inventoryUI;

  [SerializeField] private InventoryItem[] inventoryItems;
  public int inventorySize = 12;

  private void Start() {
    inventoryItems = new InventoryItem[inventorySize];
    for (int i = 0; i < inventorySize; i++) {
      inventoryItems[i] = InventoryItem.GetEmptyItem();
    }
    PrepareUI();

    controls = GameManager.Instance.controls;
    controls.Player.ToggleInventory.performed += _ => {
      if (inventoryUI.gameObject.activeSelf) {
        inventoryUI.Hide();
      } else {
        inventoryUI.Show();
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
    var inventoryItem = inventoryItems[index];
    if(inventoryItem.IsEmpty)
      return;

    var item = inventoryItem.item;
    inventoryUI.UpdateDescription(item.icon, item.name, item.description);
  }
  private void HandleItemSwap(int indexA, int indexB) { }
  private void HandleDrag(int index) { }
  private void HandleItemActionsRequest(int index) { }



  public bool Add(Item item, int quantity) {
    for (int i = 0; i < inventoryItems.Length; i++) {
      if (inventoryItems[i].IsEmpty) {
        inventoryItems[i] = new InventoryItem {
          item = item,
          quantity = quantity,
        };

        onItemChangedCallback?.Invoke();
        return true;
      }
      continue;
    }

    Debug.Log("not enough room");
    return false;
  }

  public void Remove(int slotIndex) {
    inventoryItems[slotIndex] = InventoryItem.GetEmptyItem();
    onItemChangedCallback?.Invoke();
  }

  // Return a dictionary object where empty items are not added to the dictionary at all.
  public Dictionary<int, InventoryItem> GetInventoryState() {
    Dictionary<int, InventoryItem> dict = new();
    for (int i = 0; i < inventoryItems.Length; i++) {
      if (inventoryItems[i].IsEmpty)
        continue;
      dict[i] = inventoryItems[i];
    }
    return dict;
  }
}

[Serializable]
public struct InventoryItem {
  public Item item;
  public int quantity;

  public bool IsEmpty => item == null;

  public InventoryItem SetQuantity(int newQuantity) {
    return new InventoryItem {
      item = this.item,
      quantity = newQuantity,
    };
  }

  public static InventoryItem GetEmptyItem() => new InventoryItem {
    item = null,
    quantity = 0,
  };
}