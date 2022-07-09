using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationComponent : MonoBehaviour {
  protected Animator animator;

  protected void Start() {
    animator = GetComponent<Animator>();
  }
}
