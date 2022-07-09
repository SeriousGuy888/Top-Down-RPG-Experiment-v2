using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : HealthBar {
  public Vector3 offset;

  private void Update() {
    slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
  }

  public new void SetHealth(float health, float maxHealth) {
    base.SetHealth(health, maxHealth);
    slider.gameObject.SetActive(health < maxHealth);
  }
}
