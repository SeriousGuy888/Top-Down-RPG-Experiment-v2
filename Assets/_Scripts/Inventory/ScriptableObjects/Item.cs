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

  public List<ItemPropertyData> defaultPropertiesList;
}

public interface IDestroyableItem {

}

public interface IItemAction {
  public string Name { get; }
  public AudioClip SFX { get; }

  bool Perform(GameObject obj, List<ItemPropertyData> itemState);
}

[Serializable]
public class ModifierData {
  public CharacterStatModifier statModifier;
  public float value;
}

[Serializable]
public struct ItemPropertyData : IEquatable<ItemPropertyData> {
  public ItemProperty property;
  public float value;

  public bool Equals(ItemPropertyData other) => 
    other.property == property;
}