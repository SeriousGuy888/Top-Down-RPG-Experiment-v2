using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using WorldGeneration;

public class GameManager : MonoBehaviour {
  public static GameManager Instance;

  public Player player;
  public HealthBar healthBar;

  public InputMaster controls;
  public bool isPointerOverUI;

  public MapGenerator mapGenerator;

  private void Awake() {
    Instance = this;
    
    controls = new();
    controls.Enable();

    mapGenerator.GenerateMap();
  }

  private void Update() {
    isPointerOverUI = EventSystem.current.IsPointerOverGameObject();
  }
}
