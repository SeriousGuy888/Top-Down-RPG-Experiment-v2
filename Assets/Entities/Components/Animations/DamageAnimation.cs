using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAnimation : AnimationComponent {
  [SerializeField] private float totalAnimationTime = 0.5f;

  private Collider2D entityCollider;
  private SpriteRenderer spriteRenderer;

  private new void Start() {
    base.Start();
    entityCollider = GetComponent<Collider2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
  }

  public void PlayDamageAnimation() {
    StopAllCoroutines();
    animator.SetTrigger("damage");
    StartCoroutine(DamageEffectCoroutine());
  }


  private IEnumerator DamageEffectCoroutine() {
    float elapsedTime = 0f;
    float fadeTime = totalAnimationTime / 2;
    while (elapsedTime < totalAnimationTime) {
      elapsedTime += Time.deltaTime;

      // A /\ shaped equation. | 0=>0 | 0.25=>0.5 | 0.5=>1 | 0.75=>0.5 | 1=>0 |
      // Starts at white, lerps to red at halfway, returns to white at the end
      float lerpAmt = 1 - (Mathf.Abs(elapsedTime - fadeTime) / fadeTime) * 2;
      spriteRenderer.color = Color.Lerp(Color.white, Color.red, lerpAmt);

      yield return null;
    }

    spriteRenderer.color = Color.white;
  }
}
