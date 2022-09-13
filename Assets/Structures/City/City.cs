using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour {
  private Vector2Int position;

  public void Init(Vector2Int position) {
    this.position = position;
  }
}
