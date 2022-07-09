using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
  private Image healthBar;

  public float maxHealth;
  public float health;

  private void Start() {
    healthBar = GetComponent<Image>();
  }

  private void Update() {
    healthBar.fillAmount = health / maxHealth;
  }

  public void SetHealth(float health) {
    this.health = health;
  }

  public void SetMaxHealth(float maxHealth) {
    this.maxHealth = maxHealth;
    SetHealth(maxHealth);
  }
}
