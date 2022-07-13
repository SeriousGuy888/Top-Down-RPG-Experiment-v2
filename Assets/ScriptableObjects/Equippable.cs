using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equippable", menuName = "Inventory/Equippable")]
public class Equippable : Item {
  public EquipmentSlot slot;

  public int defenceModifier;
  public int attackModifier;

  public override void Use() {
    base.Use();
    GameManager.Instance.player.equipment.Equip(this);
    GameManager.Instance.player.inventory.Remove(this);
  }
}

public enum EquipmentSlot {
  Head,
  Chest,
  Legs,
  Feet,
  Mainhand,
  Offhand,
}
