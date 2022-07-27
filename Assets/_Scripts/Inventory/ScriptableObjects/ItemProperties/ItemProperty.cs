using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Property", menuName = "Inventory/Property")]
public class ItemProperty : ScriptableObject {
  [field: SerializeField]
  public string Name { get; private set; }
}
