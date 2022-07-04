using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour {
  public Player player;
  public Collider2D swordCollider;
  public float damage = 3;

  public enum AttackDirection {
    Left = -1,
    Right = 1,
  }

  private void Start() {
    swordCollider = GetComponent<Collider2D>();
  }

  public void Attack(AttackDirection direction) {
    swordCollider.transform.localScale = new Vector3((float)direction, 1, 1);
    swordCollider.enabled = true;
    DamageOverlappingEnemies();
  }

  public void StopAttack() {
    swordCollider.enabled = false;
  }

  private void DamageOverlappingEnemies() {
    // Get all overlapping colliders => results
    ContactFilter2D filter = new ContactFilter2D();
    filter.useTriggers = true;
    List<Collider2D> results = new List<Collider2D>();
    swordCollider.OverlapCollider(filter, results);

    // Loop through all the colliders that overlap,
    // damage all those tagged as Enemy
    foreach (var collider in results) {
      if (collider.tag == "Enemy") {
        Health enemyHealth = collider.GetComponent<Health>();
        if (enemyHealth != null) {
          enemyHealth.TakeDamage(damage, player.gameObject);
        }
      }
    }
  }
}
