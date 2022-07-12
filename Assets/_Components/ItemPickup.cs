using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour {
  private Player player;
  private BoxCollider2D boxCollider;

  public Item item;

  private void Start() {
    player = GameManager.Instance.player;
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.transform == player.transform) {
      PickUp();
    }
  }

  private void PickUp() {
    Debug.Log("Picking up " + item.name);
    bool pickupSuccess = GameManager.Instance.player.inventory.Add(item);
    if (pickupSuccess)
      Destroy(gameObject);
  }
}
