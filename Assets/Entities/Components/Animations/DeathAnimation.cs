using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : AnimationComponent {
  public void PlayDeathAnimation() {
    animator.SetTrigger("die");
  }
}
