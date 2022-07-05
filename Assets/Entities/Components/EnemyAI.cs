using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
  public Vector2? spawnpoint;
  public Player targetPlayer;
  public bool targetSighted = false;

  public Creature creature;

  [Header("Context Based Steering")]
  public float maxSpeed = 350;
  public float steerForce = 0.1f;
  public float lookAhead = 2;
  public int rayCount = 8;

  private Vector2[] rayDirections;
  private float[] interest;
  private float[] danger;

  private Vector2 chosenDir = Vector2.zero;

  private void Awake() {
    rayDirections = new Vector2[rayCount];
    interest = new float[rayCount];
    danger = new float[rayCount];

    for (int i = 0; i < rayCount; i++) {
      float angle = (2 * Mathf.PI) / rayCount * i;
      Vector2 vec = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
      rayDirections[i] = vec;
    }
  }

  private void Start() {
    targetPlayer = GameManager.Instance.player;
  }

  private void FixedUpdate() {
    setInterest();
    setDanger();
    chooseDirection();

    creature.SetMovement(chosenDir);
  }

  private void setInterest() {
    if (targetPlayer == null) {
      setDefaultInterest();
      return;
    }

    Vector2 vecTowardsTarget = (targetPlayer.transform.position - transform.position).normalized;
    for (int i = 0; i < rayCount; i++) {
      var dotProduct = Vector2.Dot(rayDirections[i], vecTowardsTarget);
      interest[i] = Mathf.Max(0, dotProduct);
    }
  }

  private void setDefaultInterest() {
    for (int i = 0; i < rayCount; i++) {
      interest[i] = 1;
    }
  }

  private void setDanger() {
    for (int i = 0; i < rayCount; i++) {
      RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirections[i], lookAhead, new LayerMask());
      danger[i] = hit.collider != null ? 1 : 0;
    }
  }

  private void chooseDirection() {
    for (int i = 0; i < rayCount; i++) {
      if(danger[i] > 0)
        interest[i] = 0;
    }

    chosenDir = Vector2.zero;
    for (int i = 0; i < rayCount; i++) {
      chosenDir += rayDirections[i] * interest[i];
    }
    chosenDir.Normalize();
  }
}
