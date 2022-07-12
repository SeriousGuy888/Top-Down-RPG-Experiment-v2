using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {
  public static GameManager Instance;

  public Player player;
  public HealthBar healthBar;

  public InputMaster controls;
  public bool isPointerOverUI;

  private void Awake() {
    Instance = this;
    
    controls = new();
    controls.Enable();
  }

  private void Update() {
    isPointerOverUI = EventSystem.current.IsPointerOverGameObject();
  }
}
