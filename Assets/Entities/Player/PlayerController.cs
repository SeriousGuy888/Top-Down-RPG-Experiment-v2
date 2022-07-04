using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Creature {

  public float moveSpeed = 0.5f;
  public ContactFilter2D raycastFilter;
  private Vector2 moveInput;
  private bool canMove = true;

  private Rigidbody2D rb;

  public SwordAttack swordAttack;


  private new void Start() {
    base.Start();
    rb = GetComponent<Rigidbody2D>();
  }

  private void FixedUpdate() {
    bool success = TryMove(moveInput);
    if (!success) success = TryMove(new Vector2(moveInput.x, 0));
    if (!success) success = TryMove(new Vector2(0, moveInput.y));

    animator.SetBool("isMoving", success);
  }

  private bool TryMove(Vector2 moveVec) {
    if (moveVec == Vector2.zero || !canMove)
      return false;

    // Don't mirror if moving right, mirror if moving left
    // Don't change mirroring if no horizontal movement
    if (moveVec.x > 0)
      spriteRenderer.flipX = false;
    else if (moveVec.x < 0)
      spriteRenderer.flipX = true;

    float moveFactor = moveSpeed * Time.fixedDeltaTime;

    List<RaycastHit2D> results = new List<RaycastHit2D>();
    int collisionCount = rb.Cast(moveVec, raycastFilter, results, moveVec.magnitude * moveFactor);
    // if (collisionCount > 0)
    //   Debug.Log(results[0].collider.name);

    if (collisionCount == 0) {
      rb.MovePosition(rb.position + moveVec * moveFactor);
      return true;
    }
    return false;
  }

  private void OnMove(InputValue value) {
    Vector2 direction = value.Get<Vector2>();
    moveInput = direction;
  }

  private void OnFire() {
    animator.SetTrigger("swordAttack");
  }


  public void SwordAttackStart() {
    canMove = false;
  }
  public void SwordAttackDealDamage() {
    swordAttack.Attack(spriteRenderer.flipX
      ? SwordAttack.AttackDirection.Left
      : SwordAttack.AttackDirection.Right);
  }
  public void SwordAttackStop() {
    canMove = true;
    swordAttack.StopAttack();
  }
}
