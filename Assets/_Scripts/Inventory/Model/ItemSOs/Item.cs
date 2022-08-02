using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject {
  new public string name = "New Item";
  [TextArea] public string description;
  public Sprite icon = null;

  // Instances of the same instance of this scriptable object have the same ID.
  // Therefore, this is used to compare whether two items are of the same type.
  public int ID => GetInstanceID();

  public bool isStackable;
  public int maxStackSize = 1;

  // The default properties (eg. durability) for this item
  public List<ItemProperty> defaultPropertiesList;

  // What types of slots this item can be put in
  public List<ItemAssignedSlot> itemAllowedSlots;

  public bool IsValidSlot(List<ItemAssignedSlot> slotAccepts) {
    if(slotAccepts == null)
      return true;

    foreach(var loopSlotType in itemAllowedSlots) {
      if(slotAccepts.Contains(loopSlotType))
        return true;
    }
    return false;
  }
}

public enum ItemAssignedSlot {
  Head,
  Chest,
  Legs,
  Feet,
  Offhand,
  Mainhand,
}

public interface IDestroyableItem {

}

public interface IItemAction {
  public string Name { get; }
  public AudioClip SFX { get; }

  bool Perform(GameObject obj, List<ItemProperty> itemState);
}

[Serializable]
public class ModifierData {
  public CharacterStatModifier statModifier;
  public float value;
}