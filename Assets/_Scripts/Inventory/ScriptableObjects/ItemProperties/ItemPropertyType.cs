using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Property Type", menuName = "Inventory/Item Property Type")]
public class ItemPropertyType : ScriptableObject {
  [field: SerializeField]
  public string Name { get; private set; }
}
