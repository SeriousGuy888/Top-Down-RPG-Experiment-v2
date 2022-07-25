using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Modifiers/Health Modifier")]
public class HealthModifier : CharacterStatModifier {
  public override void AffectCharacter(GameObject character, float val) {
    Health health = character.GetComponent<Health>();
    if (health == null)
      return;

    health.HP += (int)val;
  }
}
