using System;

[Serializable]
public struct ItemProperty : IEquatable<ItemProperty> {
  public ItemPropertyType property;
  public float value;

  public bool Equals(ItemProperty other) =>
    other.property == property;
}