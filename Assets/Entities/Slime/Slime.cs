using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Creature {
  public Player targetPlayer;

  private new void Start() {
    base.Start();
    targetPlayer = GameManager.Instance.player;
  }

  private void FixedUpdate() {
    // if(targetPlayer == null)
    //   return;

    // Vector2 vecBetween = targetPlayer.transform.position - transform.position;
    // TryMove(vecBetween.normalized);
  }
}
