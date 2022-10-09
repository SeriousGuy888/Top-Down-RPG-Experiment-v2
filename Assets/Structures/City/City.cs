using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour {
  public float interactDistance = 1.5f;

  private Vector2Int position;
  private InputMaster controls;

  private void Awake() {
    controls = GameManager.Instance.controls;
    controls.Player.Interact.performed += _ => {
      var playerPos = GameManager.Instance.player.transform.position;
      var dist = Vector2.Distance(playerPos, transform.position);
      if(dist > interactDistance) {
        return;
      }
      Debug.Log("pog");
    };
  }

  public void Init(Vector2Int position) {
    this.position = position;
  }
}
