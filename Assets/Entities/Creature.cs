using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {
  [SerializeField] private float moveSpeed = 0.5f;
  [SerializeField] private ContactFilter2D raycastFilter;

  protected SpriteRenderer spriteRenderer;
  protected Animator animator;
  protected Collider2D entityCollider;
  protected Rigidbody2D rb;

  protected bool canMove = true;
  protected Vector2 moveInput;


  protected void Start() {
    spriteRenderer = GetComponent<SpriteRenderer>();
    animator = GetComponent<Animator>();
    entityCollider = GetComponent<Collider2D>();
    rb = GetComponent<Rigidbody2D>();
  }

  private void FixedUpdate() {
    bool success = TryMove(moveInput);
    if (!success) success = TryMove(new Vector2(moveInput.x, 0));
    if (!success) success = TryMove(new Vector2(0, moveInput.y));
  }

  public void SetMovement(Vector2 vec) {
    moveInput = vec;
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
}
