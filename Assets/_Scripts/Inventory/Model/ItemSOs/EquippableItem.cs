using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equippable Item", menuName = "Inventory/Item Types/Equippable")]
public class EquippableItem : Item, IDestroyableItem, IItemAction {
  public int defenceModifier;
  public int attackModifier;

  public string Name => "Equip";
  public AudioClip SFX { get; private set; }

  public bool Perform(GameObject obj, List<ItemProperty> itemState = null) {
    throw new System.NotImplementedException();
  }
}