using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Creature {
  public SwordAttack swordAttack;

  private void FixedUpdate() {
    bool success = TryMove(moveInput);
    if (!success) success = TryMove(new Vector2(moveInput.x, 0));
    if (!success) success = TryMove(new Vector2(0, moveInput.y));
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
