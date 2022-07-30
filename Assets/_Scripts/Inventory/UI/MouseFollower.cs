using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseFollower : MonoBehaviour {
  private Canvas canvas;
  private Camera cam;

  private InventorySlot floatingItem;

  private void Awake() {
    canvas = transform.root.GetComponent<Canvas>();
    cam = Camera.main;
    floatingItem = GetComponentInChildren<InventorySlot>();
  }

  public void SetData(Sprite sprite, int quantity) {
    floatingItem.SetData(sprite, quantity);
  }

  private void Update() {
    Vector2 position;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(
      (RectTransform)canvas.transform,
      Mouse.current.position.ReadValue(),
      canvas.worldCamera,
      out position
    );

    transform.position = canvas.transform.TransformPoint(position);

    // Vector2 position = cam.ScreenToViewportPoint(Mouse.current.position.ReadValue());
    // transform.position = position;
  }

  public void Toggle(bool active) {
    gameObject.SetActive(active);
  }
}
