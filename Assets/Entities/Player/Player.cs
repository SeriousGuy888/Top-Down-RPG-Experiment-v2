using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Creature {
  public SwordAttack swordAttack;

  private new void Start() {
    base.Start();
    GameManager.Instance.healthBar.SetMaxHealth(this.health.maxHealth);
  }

  private void OnMove(InputValue value) {
    Vector2 direction = value.Get<Vector2>();
    moveInput = direction;
  }

  private void OnFire() {
    animator.SetTrigger("swordAttack");
  }


  public void SwordAttackStart() {
    stunned = true;
  }
  public void SwordAttackDealDamage() {
    swordAttack.Attack(spriteRenderer.flipX
      ? SwordAttack.AttackDirection.Left
      : SwordAttack.AttackDirection.Right);
  }
  public void SwordAttackStop() {
    stunned = false;
    swordAttack.StopAttack();
  }

  public void UpdateHealthBarValue() {
    GameManager.Instance.healthBar.SetHealth(this.health.HP);
  }
}
