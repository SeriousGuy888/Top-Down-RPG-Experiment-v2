using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryData : MonoBehaviour {
  [SerializeField] private InventoryItem[] inventoryItems;

  public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdate;


  public void Init(int size) {
    inventoryItems = new InventoryItem[size];
    for (int i = 0; i < size; i++) {
      inventoryItems[i] = InventoryItem.GetEmptyItem();
    }
  }

  public bool Add(Item item, int quantity) {
    for (int i = 0; i < inventoryItems.Length; i++) {
      if (inventoryItems[i].IsEmpty) {
        inventoryItems[i] = new InventoryItem {
          item = item,
          quantity = quantity,
        };

        return true;
      }
      continue;
    }

    Debug.Log("not enough room");
    return false;
  }

  public void Remove(int slotIndex) {
    inventoryItems[slotIndex] = InventoryItem.GetEmptyItem();
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

  public InventoryItem GetItem(int index) {
    if(inventoryItems.Length > index) {
      return inventoryItems[index];
    }
    return InventoryItem.GetEmptyItem();
  }

  public void SwapItems(int indexA, int indexB) {
    var temp = inventoryItems[indexA];
    inventoryItems[indexA] = inventoryItems[indexB];
    inventoryItems[indexB] = temp;
    AnnounceChange();
  }

  private void AnnounceChange() {
    OnInventoryUpdate?.Invoke(GetInventoryState());
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