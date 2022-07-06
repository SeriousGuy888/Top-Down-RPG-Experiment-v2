using System.ComponentModel.Design.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Health : MonoBehaviour {
  public GameObject floatingTextPrefab;

  public UnityEvent<GameObject> OnDamageFromSource;
  public UnityEvent OnDeath;

  public float maxHealth = 20f;

  private float hp;
  public float HP {
    set {
      hp = value;
      if (hp <= 0) {
        OnDeath.Invoke();
        StartCoroutine(ForceDieAfterTimeout());
      } else {
        // animator.SetTrigger("damage");
        // StartCoroutine(DamageEffectCoroutine());
      }
    }
    get {
      return hp;
    }
  }

  private void Start() {
    HP = maxHealth;
  }


#nullable enable
  public void TakeDamage(float damage, GameObject? damageSource) {
    HP -= damage;

    if(floatingTextPrefab != null) {
      ShowFloatingText(damage.ToString(), Color.red);
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
