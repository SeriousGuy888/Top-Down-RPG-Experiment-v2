using System.Net.NetworkInformation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour {
  public int slotCount;
  public Equippable[] items;

  private Inventory inventory;

  private void Start() {
    inventory = GameManager.Instance.player.inventory;

    int actualSlotCount = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
    if (actualSlotCount != slotCount)
      Debug.LogWarning("Incorrect number of equipment slots!");
    slotCount = actualSlotCount;

    items = new Equippable[slotCount];
  }

  public void Equip(Equippable item) {
    int slotIndex = (int)item.slot;

    // Return any item already in that slot to the inventory
    if (items[slotIndex] != null) {
      inventory.Add(items[slotIndex]);
    }

    items[slotIndex] = item;
  }

  public void Unequip(int slotIndex) {
    Equippable item = items[slotIndex];
    if (item != null) {
      bool success = inventory.Add(item);
      if (success)
        items[slotIndex] = null;
    }
  }
}
