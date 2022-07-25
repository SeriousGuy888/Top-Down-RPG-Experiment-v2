using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Health : MonoBehaviour {
  public GameObject floatingTextPrefab;

  public UnityEvent<float, float> OnHealthChange;
  public UnityEvent<GameObject> OnDamageFromSource;
  public UnityEvent OnDeath;

  private float lastDamageTime;
  private float invincibilityTimeOnDamage = 0.5f;

  public float maxHealth = 20f;

  private float hp;
  public float HP {
    set {
      hp = Math.Min(value, maxHealth);
      OnHealthChange.Invoke(hp, maxHealth);

      if (hp <= 0) {
        OnDeath.Invoke();
        StartCoroutine(ForceDieAfterTimeout());
      }
    }
    get {
      return hp;
    }
  }

  private void Start() {
    HP = maxHealth;
    lastDamageTime = 0;
  }


#nullable enable
  public void TakeDamage(float damage, GameObject? damageSource) {
    if(Time.time - lastDamageTime < invincibilityTimeOnDamage)
      return;

    HP -= damage;
    lastDamageTime = Time.time;

    if(floatingTextPrefab != null) {
      ShowFloatingText((Mathf.Round(damage * 10) / 10).ToString(), Color.red);
    }

    if(damageSource != null)
      OnDamageFromSource.Invoke(damageSource);
  }
#nullable disable

  private void ShowFloatingText(String text, Color col) {
    GameObject obj = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity, transform);
    TextMeshPro textComp = obj.GetComponent<TextMeshPro>();
    textComp.text = text;
    textComp.color = col;
  }


  public IEnumerator ForceDieAfterTimeout() {
    yield return new WaitForSeconds(2f);
    if(gameObject != null) {
      RemoveObject();
    }
  }
  // Only used at end of death animation
  public void RemoveObject() {
    Destroy(gameObject);
  }
}
