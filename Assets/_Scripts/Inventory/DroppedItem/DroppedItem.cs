using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DroppedItem : MonoBehaviour {
  private Player player;
  private BoxCollider2D boxCollider;
  private SpriteRenderer spriteRenderer;

  public Item item;
  public int quantity = 1;

  private void Start() {
    boxCollider = GetComponent<BoxCollider2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();

    if(Application.isPlaying) {
      player = GameManager.Instance.player;
    }
  }

  private void Update() {
    if(spriteRenderer != null && item != null) {
      spriteRenderer.sprite = item.icon;
    }
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.transform == player.transform) {
      PickUp();
    }
  }

  private void PickUp() {
    int quantityRemaining = Inventory.Instance.inventoryData.Add(item, quantity);
    if(quantityRemaining == quantity) // if no items were picked up
      return;
    quantity = quantityRemaining;

    bool itemsRemain = quantityRemaining != 0;
    boxCollider.enabled = itemsRemain; // disable collision only if no items remain
    StartCoroutine(PlayPickupAnimation(itemsRemain));
  }

  private IEnumerator PlayPickupAnimation(bool itemsRemain) {
    Vector2 currentScale = transform.localScale;
    Vector2 zeroScale = Vector2.zero;

    float elapsed = 0;
    float animDuration = 0.3f;
    while(elapsed < animDuration) {
      elapsed += Time.deltaTime;
      transform.localScale = Vector2.Lerp(currentScale, zeroScale, elapsed / animDuration);
      yield return null;
    }

    if(!itemsRemain)
      Destroy(gameObject);
    else
      transform.localScale = currentScale;
  }
}
