using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {
  public Vector2? spawnpoint;
  public Player player;
  public bool targetSighted = false;

  public Creature creature;

  private Rigidbody2D rb;


  [Header("NavMeshPlus Pathfinding")]
  private NavMeshPath path;
  private Vector2 vecToPlayer;
  private Vector2 nextWaypoint;
  private float timeSinceLastPathRecalc;

  [Header("Context Based Steering")]
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
    player = GameManager.Instance.player;

    rb = GetComponent<Rigidbody2D>();
    path = new NavMeshPath();
    timeSinceLastPathRecalc = 0;
  }

  private void FixedUpdate() {
    if (creature.stunned)
      return;

    RaycastHit2D[] hits = new RaycastHit2D[1];
    vecToPlayer = player.transform.position - transform.position;
    rb.Cast(vecToPlayer.normalized, hits, 50);
    targetSighted = (hits[0].collider != null) && (hits[0].transform.tag == "Player");

    if (!targetSighted) {
      UpdatePath();
    }

    setInterest();
    setAvoid();
    setDanger();

    if(vecToPlayer.magnitude <= 1f) {
      float dmg = UnityEngine.Random.Range(1f, 3f);
      player.health.TakeDamage(dmg, gameObject);
    }


    if (drawDebugLines) {
      for (int i = 0; i < rayCount; i++) {
        Debug.DrawRay(transform.position, rayDirections[i] * interest[i], Color.green, Time.deltaTime);
        Debug.DrawRay(transform.position, rayDirections[i] * avoid[i], Color.yellow, Time.deltaTime);
        Debug.DrawRay(transform.position, rayDirections[i] * danger[i], Color.red, Time.deltaTime);
      }

      Debug.DrawLine(transform.position, nextWaypoint, Color.yellow, Time.deltaTime);
    }

    chooseDirection();

    creature.SetMovement(chosenDir);
    if (drawDebugLines) {
      Debug.DrawRay(transform.position, chosenDir, Color.blue, Time.deltaTime);
    }
  }

  private void UpdatePath() {
    timeSinceLastPathRecalc += Time.deltaTime;
    if (timeSinceLastPathRecalc > 1.0f) {
      timeSinceLastPathRecalc -= 1.0f;
      NavMesh.CalculatePath(transform.position, player.transform.position, NavMesh.AllAreas, path);
    }
    if (drawDebugLines) {
      for (int i = 0; i < path.corners.Length - 1; i++)
        Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.yellow, Time.deltaTime);
    }

    if (path.corners.Length > 1)
      nextWaypoint = path.corners[1];
  }


  private void setInterest() {
    if (nextWaypoint == null && !targetSighted) {
      setDefaultInterest();
      return;
    }

    Vector2 vecTowardsTarget;
    if (targetSighted) {
      vecTowardsTarget = vecToPlayer.normalized;
    } else {
      vecTowardsTarget = (nextWaypoint - (Vector2)transform.position).normalized;
    }

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

      if (hit.collider != null)
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
}
