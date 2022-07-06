using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour {
  public float timeToLive = 3f;
  public Vector2 offset = new Vector2(0, 1f);
  public Vector2 posVariation = new Vector2(0.2f, 0.2f);

  private void Start() {
    transform.localPosition += (Vector3)offset + new Vector3(
      Random.Range(-posVariation.x, posVariation.x),
      Random.Range(-posVariation.y, posVariation.y),
      0
    );

    Destroy(gameObject, timeToLive);
  }
}
