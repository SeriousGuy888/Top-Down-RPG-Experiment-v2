using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Knockback : MonoBehaviour {
  [SerializeField] private Rigidbody2D rb;

  [SerializeField] private float strength = 5f;
  [SerializeField] private float delay = 0.25f;

  public UnityEvent OnStart;
  public UnityEvent OnStop;

  public void PlayKnockback(GameObject knockbackSource) {
    StopAllCoroutines();
    OnStart?.Invoke();

    Vector2 direction = (transform.position - knockbackSource.transform.position).normalized;
    rb.AddForce(direction * strength, ForceMode2D.Impulse);

    StartCoroutine(StopKnockback());
  }

  private IEnumerator StopKnockback() {
    yield return new WaitForSeconds(delay);
    rb.velocity = Vector2.zero;

    OnStop?.Invoke();
  }
}
