using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {
  public float invincibilityTimer = 0.5f;

  protected SpriteRenderer spriteRenderer;
  protected Animator animator;
  protected Collider2D entityCollder;

  private float health = 20f;
  public float Health {
    set {
      health = value;
      if (health <= 0) {
        Die();
      } else {
        animator.SetTrigger("damage");
        StartCoroutine(DamageFadeEffect());
      }
    }
    get {
      return health;
    }
  }


  protected void Start() {
    spriteRenderer = GetComponent<SpriteRenderer>();
    animator = GetComponent<Animator>();
    entityCollder = GetComponent<Collider2D>();
  }

  public void TakeDamage(float damage) {
    Health -= damage;
  }


  IEnumerator DamageFadeEffect() {
    entityCollder.enabled = false;

    float elapsedTime = 0f;
    float fadeTime = invincibilityTimer / 2;
    while (elapsedTime < invincibilityTimer) {
      elapsedTime += Time.deltaTime;

      // A /\ shaped equation. | 0=>0 | 0.25=>0.5 | 0.5=>1 | 0.75=>0.5 | 1=>0 |
      // Starts at white, lerps to red at halfway, returns to white at the end
      float lerpAmt = 1 - (Mathf.Abs(elapsedTime - fadeTime) / fadeTime) * 2;
      spriteRenderer.color = Color.Lerp(Color.white, Color.red, lerpAmt);

      yield return null;
    }

    spriteRenderer.color = Color.white;
    entityCollder.enabled = true;
  }



  public void Die() {
    animator.SetTrigger("die");
  }
  // Only used at end of death animation
  public void RemoveObject() {
    Destroy(gameObject);
  }
}
