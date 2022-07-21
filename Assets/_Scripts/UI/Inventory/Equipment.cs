using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour {
  public static Equipment Instance;
  private void Awake() {
    Instance = this;
  }

  public delegate void OnItemChanged();
  public OnItemChanged onItemChangedCallback;

  public int slotCount;
  public Equippable[] items;

  private void Start() {
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
      Inventory.Instance.Add(items[slotIndex]);
    }

    items[slotIndex] = item;
    if (onItemChangedCallback != null)
      onItemChangedCallback.Invoke();
  }

  public void Unequip(int slotIndex) {
    Equippable item = items[slotIndex];
    if (item != null) {
      bool success = Inventory.Instance.Add(item);
      if (success) {
        items[slotIndex] = null;
        if (onItemChangedCallback != null)
          onItemChangedCallback.Invoke();
      }
    }
  }
}