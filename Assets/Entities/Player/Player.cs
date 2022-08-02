using UnityEngine;
using UnityEngine.EventSystems;

public class Player : Creature {
  public SwordAttack swordAttack;
  public Inventory inventory;

  private InputMaster controls;

  private void Awake() {
    controls = GameManager.Instance.controls;

    controls.Player.Fire.performed += _ => {
      if(GameManager.Instance.isPointerOverUI)
        return;
      animator.SetTrigger("swordAttack");
    };
  }

  private new void Start() {
    base.Start();
    UpdateHealthBarValue();
  }

  private new void FixedUpdate() {
    base.FixedUpdate();

    var direction = controls.Player.Move.ReadValue<Vector2>();
    moveInput = direction;
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
