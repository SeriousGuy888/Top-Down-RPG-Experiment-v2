using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : MonoBehaviour {
  private Animator animator;

  private void Start() {
    animator = GetComponent<Animator>();
  }

  public void PlayDeathAnimation() {
    animator.SetTrigger("die");
  }
}
