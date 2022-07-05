using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
  public Vector2? spawnpoint;
  public Player targetPlayer;
  public bool targetSighted = false;

  public Creature creature;

  [Header("Context Based Steering")]
  public bool stunned = false; // will be false while knockback is in effect
  public float maxSpeed = 350;
  public float steerForce = 0.1f;
  public float lookAhead = 4;
  public int rayCount = 12;
  public ContactFilter2D avoidFilter;
  public LayerMask dangerMask;
  public bool drawDebugLines;

  private Vector2[] rayDirections;
  private float[] interest;
  private float[] avoid;
  private float[] danger;

  private Vector2 chosenDir = Vector2.zero;

  private Rigidbody2D rb;

  private void Awake() {
    rayDirections = new Vector2[rayCount];
    interest = new float[rayCount];
    avoid = new float[rayCount];
    danger = new float[rayCount];

    for (int i = 0; i < rayCount; i++) {
      float angle = (2 * Mathf.PI) / rayCount * i;
      Vector2 vec = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
      rayDirections[i] = vec;
    }
  }

  private void Start() {
    targetPlayer = GameManager.Instance.player;
    rb = GetComponent<Rigidbody2D>();
  }

  private void FixedUpdate() {
    if(stunned)
      return;

    setInterest();
    setAvoid();
    setDanger();

    if (drawDebugLines) {
      for (int i = 0; i < rayCount; i++) {
        Debug.DrawRay(transform.position, rayDirections[i] * interest[i], Color.green, Time.deltaTime);
        Debug.DrawRay(transform.position, rayDirections[i] * avoid[i], Color.yellow, Time.deltaTime);
        Debug.DrawRay(transform.position, rayDirections[i] * danger[i], Color.red, Time.deltaTime);
      }
    }

    chooseDirection();

    creature.SetMovement(chosenDir);
    if (drawDebugLines) {
      Debug.DrawRay(transform.position, chosenDir, Color.blue, Time.deltaTime);
    }
  }

  private void setInterest() {
    if (targetPlayer == null) {
      setDefaultInterest();
      return;
    }

    Vector2 vecTowardsTarget = (targetPlayer.transform.position - transform.position).normalized;
    for (int i = 0; i < rayCount; i++) {
      var dotProduct = Vector2.Dot(rayDirections[i], vecTowardsTarget);
      interest[i] = Mathf.Max(0.1f, dotProduct);
    }
  }

  private void setDefaultInterest() {
    for (int i = 0; i < rayCount; i++) {
      interest[i] = 1;
    }
  }

  private void setAvoid() {
    for (int i = 0; i < rayCount; i++) {
      // RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirections[i], lookAhead, avoidFilter);
      RaycastHit2D[] results = new RaycastHit2D[1];
      int hits = rb.Cast(rayDirections[i], avoidFilter, results, lookAhead);
      RaycastHit2D hit = results[0];

      if(hit.collider != null)
        avoid[i] = (lookAhead - hit.distance) / (lookAhead * 2);
      else
        avoid[i] = 0;
    }
  }

  private void setDanger() {
    for (int i = 0; i < rayCount; i++) {
      RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirections[i], lookAhead, dangerMask);
      danger[i] = hit.collider != null ? 1 : 0;
    }
  }

  private void chooseDirection() {
    for (int i = 0; i < rayCount; i++) {
      if (danger[i] > 0)
        interest[i] = 0;
      else {
        interest[i] -= avoid[i];
        // interest[i] = Mathf.Max(0, interest[i]);
      }
    }

    chosenDir = Vector2.zero;
    for (int i = 0; i < rayCount; i++) {
      chosenDir += rayDirections[i] * interest[i];
    }
    chosenDir.Normalize();
  }

  public void Stun() { stunned = true; }
  public void Unstun() { stunned = false; }
}
