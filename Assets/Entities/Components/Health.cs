using System.ComponentModel.Design.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {
  public UnityEvent<GameObject> OnDamageFromSource;
  public UnityEvent OnDeath;

  private float hp = 20f;
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


#nullable enable
  public void TakeDamage(float damage, GameObject? damageSource) {
    HP -= damage;
    if(damageSource != null)
      OnDamageFromSource.Invoke(damageSource);
  }
#nullable disable

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
