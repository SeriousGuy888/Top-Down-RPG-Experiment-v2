using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ItemPickup : MonoBehaviour {
  private Player player;
  private BoxCollider2D boxCollider;

  public Item item;

  private void Start() {
    if(Application.isPlaying) {
      player = GameManager.Instance.player;
    }
  }

  private void Update() {
#if UNITY_EDITOR
    var spriteRenderer = GetComponent<SpriteRenderer>();
    if(spriteRenderer != null && item != null) {
      spriteRenderer.sprite = item.icon;
    }
#endif
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.transform == player.transform) {
      PickUp();
    }
  }

  private void PickUp() {
    Debug.Log("Picking up " + item.name);
    bool pickupSuccess = Inventory.Instance.Add(item, 1);
    if (pickupSuccess)
      Destroy(gameObject);
  }
}
