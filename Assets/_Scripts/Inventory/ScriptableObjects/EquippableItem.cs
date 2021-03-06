using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equippable", menuName = "Inventory/Equippable")]
public class EquippableItem : Item {
  public EquipmentSlot slot;

  public int defenceModifier;
  public int attackModifier;

  private bool equipped = false;
}

public enum EquipmentSlot {
  Head,
  Chest,
  Legs,
  Feet,
  Mainhand,
  Offhand,
}
