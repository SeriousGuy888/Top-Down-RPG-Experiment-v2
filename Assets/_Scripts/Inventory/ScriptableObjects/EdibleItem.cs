using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Edible", menuName = "Inventory/Edible")]
public class EdibleItem : Item, IDestroyableItem, IItemAction {
  [SerializeField] private List<ModifierData> modifiersData = new();

  public string Name => "Consume";
  public AudioClip SFX { get; private set; }

  public bool Perform(GameObject character) {
    foreach(var data in modifiersData) {
      data.statModifier.AffectCharacter(character, data.value);
    }
    return true;
  }
}
