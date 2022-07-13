using System.Net.NetworkInformation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour {
  private Equippable[] currentEquipment;
  private Inventory inventory;

  private void Start() {
    inventory = GameManager.Instance.player.inventory;

    int slotCount = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
    currentEquipment = new Equippable[slotCount];
  }

  public void Equip(Equippable item) {
    int slotIndex = (int)item.slot;

    // Return any item already in that slot to the inventory
    if(currentEquipment[slotIndex] != null) {
      inventory.Add(currentEquipment[slotIndex]);
    }

    currentEquipment[slotIndex] = item;
  }

  public void Unequip(int slotIndex) {
    Equippable item = currentEquipment[slotIndex];
    if(item != null) {
      bool success = inventory.Add(item);
      if(success)
        currentEquipment[slotIndex] = null;
    }
  }
}
