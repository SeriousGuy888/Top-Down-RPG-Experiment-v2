using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Creature {
  public SwordAttack swordAttack;

  private PlayerInput playerInput;
  private InputAction moveAction;

  private new void Start() {
    base.Start();

    UpdateHealthBarValue();

    playerInput = GetComponent<PlayerInput>();
    moveAction = playerInput.actions["Move"];
  }

  private new void FixedUpdate() {
    base.FixedUpdate();

    var direction = moveAction.ReadValue<Vector2>();
    moveInput = direction;
  }

  private void OnFire() {
    animator.SetTrigger("swordAttack");
  }


  public void SwordAttackStart() {
    // stunned = true;
  }
  public void SwordAttackDealDamage() {
    swordAttack.Attack(spriteRenderer.flipX
      ? SwordAttack.AttackDirection.Left
      : SwordAttack.AttackDirection.Right);
  }
  public void SwordAttackStop() {
    // stunned = false;
    swordAttack.StopAttack();
  }

  public void UpdateHealthBarValue() {
    GameManager.Instance.healthBar.SetHealth(this.health.HP, this.health.maxHealth);
  }
}
