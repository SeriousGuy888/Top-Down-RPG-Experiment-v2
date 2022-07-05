using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Creature {
  public SwordAttack swordAttack;

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
