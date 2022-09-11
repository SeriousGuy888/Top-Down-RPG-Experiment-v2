using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using WorldGeneration;

public class GameManager : MonoBehaviour {
  public static GameManager Instance;

  public Player player;
  public HealthBar healthBar;

  public InputMaster controls;
  public bool isPointerOverUI;

  public MapGenerator mapGenerator;
  public NavMeshSurface navMeshSurface;

  private void Awake() {
    Instance = this;
    
    controls = new();
    controls.Enable();

    mapGenerator.GenerateMap();
  }

  private void Start() {
    navMeshSurface.BuildNavMeshAsync();
  }

  private void Update() {
    isPointerOverUI = EventSystem.current.IsPointerOverGameObject();
  }
}
