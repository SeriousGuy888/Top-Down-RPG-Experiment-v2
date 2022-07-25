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
}
