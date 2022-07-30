using System.Runtime.InteropServices.WindowsRuntime;
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

  /// <summary>
  ///   Adds an item to the inventory, allowing for a quantity to be specified. The
  ///   item will be stacked to other items of the same type where possible.
  /// </summary>
  /// <returns>
  ///   Quantity of items remaining after adding as much as possible to the inventory.
  ///   Returns zero if all items were successfully added to the inventory.
  /// </returns>
  public int Add(Item newItem, int quantity, List<ItemProperty> itemState = null) {
    // Loop through the inventory, looking for existing stacks of the same
    // item type. Add items to stacks to the max stack size, and then move on.
    for (int i = 0; i < inventoryItems.Length; i++) {
      // Skip this loop entirely if the item is not stackable.
      if (!newItem.isStackable)
        break;

      var existingItemStack = inventoryItems[i];

      if (existingItemStack.IsEmpty)
        continue;

      // If stack is of a different item type to the incoming item
      if (existingItemStack.item.ID != newItem.ID)
        continue;

      // If stack is already full.
      if (existingItemStack.quantity >= newItem.maxStackSize)
        continue;


      int quantitySum = existingItemStack.quantity + quantity;
      if (quantitySum > newItem.maxStackSize) {
        // If there is not enough room in this stack

        int spaceRemainingInStack = newItem.maxStackSize - existingItemStack.quantity;
        inventoryItems[i] = existingItemStack.SetQuantity(newItem.maxStackSize);
        quantity -= spaceRemainingInStack;
      } else {
        // If there is enough room in this stack for all the items, add it to the
        // stack and return a success.

        inventoryItems[i] = existingItemStack.SetQuantity(quantitySum);
        AnnounceChange();
        return 0;
      }
    }

    // At this point, if not returned, there are still items left to distribute,
    // and no stacks of that item that have space available, or the incoming item
    // is not stackable.


    for (int i = 0; i < inventoryItems.Length; i++) {
      if (!inventoryItems[i].IsEmpty)
        continue;

      int newItemStackQuantity = Mathf.Min(quantity, newItem.maxStackSize);
      quantity -= newItemStackQuantity;

      inventoryItems[i] = new InventoryItem {
        item = newItem,
        quantity = newItemStackQuantity,
        itemState = itemState ?? new(),
      };

      if (quantity <= 0)
        break;
    }

    AnnounceChange();
    return quantity;
  }

  public void Remove(int slotIndex) {
    inventoryItems[slotIndex] = InventoryItem.GetEmptyItem();
    AnnounceChange();
  }
  public void Remove(int slotIndex, int removeQuantity) {
    var invItem = GetItem(slotIndex);
    if (invItem.IsEmpty)
      return;

    int newQuantity = invItem.quantity - removeQuantity;
    if (newQuantity <= 0) {
      Remove(slotIndex);
    } else {
      inventoryItems[slotIndex] = invItem.SetQuantity(newQuantity);
      AnnounceChange();
    }
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
    if (inventoryItems.Length > index) {
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
  public List<ItemProperty> itemState;

  public bool IsEmpty => item == null;

  /// <summary>
  /// (!!!) This method does not modify the struct! 
  /// </summary>
  /// <returns>
  /// A new instance of the struct with the modified quantity.
  /// </returns>
  public InventoryItem SetQuantity(int newQuantity) {
    return new InventoryItem {
      item = this.item,
      quantity = newQuantity,
      itemState = new(),
    };
  }

  public static InventoryItem GetEmptyItem() => new InventoryItem {
    item = null,
    quantity = 0,
    itemState = new(),
  };
}