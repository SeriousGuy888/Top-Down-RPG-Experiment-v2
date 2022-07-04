using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {
  [SerializeField] private float invincibilityTimer = 0.5f;
  [SerializeField] private float moveSpeed = 0.5f;
  [SerializeField] private ContactFilter2D raycastFilter;

  protected SpriteRenderer spriteRenderer;
  protected Animator animator;
  protected Collider2D entityCollder;
  protected Rigidbody2D rb;

  protected bool canMove = true;
  protected Vector2 moveInput;

  private float health = 20f;
  public float Health {
    set {
      health = value;
      if (health <= 0) {
        Die();
      } else {
        animator.SetTrigger("damage");
        StartCoroutine(DamageEffectCoroutine());
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
    rb = GetComponent<Rigidbody2D>();
  }

  protected bool TryMove(Vector2 moveVec) {
    if (moveVec == Vector2.zero || !canMove) {
      animator.SetBool("isMoving", false);
      return false;
    }

    // Don't mirror if moving right, mirror if moving left
    // Don't change mirroring if no horizontal movement
    if (moveVec.x > 0)
      spriteRenderer.flipX = false;
    else if (moveVec.x < 0)
      spriteRenderer.flipX = true;

    float moveFactor = moveSpeed * Time.fixedDeltaTime;

    List<RaycastHit2D> results = new List<RaycastHit2D>();
    int collisionCount = rb.Cast(moveVec, raycastFilter, results, moveVec.magnitude * moveFactor);

    animator.SetBool("isMoving", collisionCount == 0);
    if (collisionCount == 0) {
      rb.MovePosition(rb.position + moveVec * moveFactor);
      return true;
    }
    return false;
  }


  #nullable enable
  public void TakeDamage(float damage, Creature? attacker) {
    Health -= damage;
    if(attacker != null) {
      Vector2 knockDirection = transform.position - attacker.transform.position;
      Vector2 knockVelocity = knockDirection.normalized * 2;

      // rb.isKinematic = false;
      rb.AddForce(knockVelocity, ForceMode2D.Impulse);
      StartCoroutine(KnockbackCoroutine());
    }
  }

  private IEnumerator KnockbackCoroutine() {
    if(this.Health > 0) {
      yield return new WaitForSeconds(0.5f);
      rb.velocity = Vector2.zero;
      // rb.isKinematic = true;
    }
  }

  private IEnumerator DamageEffectCoroutine() {
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
