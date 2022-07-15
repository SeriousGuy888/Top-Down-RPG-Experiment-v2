using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equippable", menuName = "Inventory/Equippable")]
public class Equippable : Item {
  public EquipmentSlot slot;

  public int defenceModifier;
  public int attackModifier;

  private bool equipped = false;

  public override void Use() {
    base.Use();
    if (equipped) {
      GameManager.Instance.player.equipment.Unequip((int)this.slot);
      equipped = false;
    } else {
      GameManager.Instance.player.equipment.Equip(this);
      GameManager.Instance.player.inventory.Remove(this);
      equipped = true;
    }
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
