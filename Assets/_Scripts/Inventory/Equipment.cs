using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour {
  public static Equipment Instance;
  private void Awake() {
    Instance = this;
  }

  public int slotCount;
  public EquippableItem[] items;

  private void Start() {
    int actualSlotCount = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
    if (actualSlotCount != slotCount)
      Debug.LogWarning("Incorrect number of equipment slots!");
    slotCount = actualSlotCount;

    items = new EquippableItem[slotCount];
  }

  public void Equip(EquippableItem item) {
    int slotIndex = (int)item.slot;

    // Return any item already in that slot to the inventory
    if (items[slotIndex] != null) {
      Inventory.Instance.inventoryData.Add(items[slotIndex], 1);
    }

    items[slotIndex] = item;
    // if (onItemChangedCallback != null)
    //   onItemChangedCallback.Invoke();
  }

  public void Unequip(int slotIndex) {
    EquippableItem item = items[slotIndex];
    if (item != null) {
      int remainingQuantity = Inventory.Instance.inventoryData.Add(item, 1);
      if (remainingQuantity == 0)
        items[slotIndex] = null;
    }
  }
}
