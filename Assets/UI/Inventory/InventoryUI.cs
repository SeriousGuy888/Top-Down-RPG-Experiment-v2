using System.Net.Sockets;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class InventoryUI : MonoBehaviour {
  public Inventory inventory;
  public Equipment equipment;
  public GameObject slotPrefab;
  private InputMaster controls;

  public Transform invSlotsContainer;
  private InventorySlot[] invSlots;

  public Transform equipSlotsContainer;
  public Sprite[] equipSlotsIcons;
  private InventorySlot[] equipSlots;

  public bool inventoryOpen = false;


  private void Start() {
    if (!Application.isPlaying)
      return;

    ToggleInventory(false);

    controls = GameManager.Instance.controls;
    controls.Player.ToggleInventory.performed += _ => ToggleInventory();
    invSlotsContainer.gameObject.SetActive(false);

    inventory.onItemChangedCallback += UpdateUI;

    SetupSlots();
  }

#region Automatic UI Generation in Editor
  private void Update() {
    if (Application.isPlaying)
      return;

    GenerateRequiredSlots(invSlotsContainer, Mathf.Max(inventory.space, 0));
    GenerateRequiredSlots(equipSlotsContainer, Mathf.Max(equipment.slotCount, 0));

    SetupSlots();
  }

  private void GenerateRequiredSlots(Transform container, int neededSlots) {
    int existingSlots = container.childCount;

    while (existingSlots > neededSlots) {
      DestroyImmediate(container.GetChild(neededSlots).gameObject);
      existingSlots--;
    }
    if (existingSlots < neededSlots) {
      for (int i = 0; i < neededSlots - existingSlots; i++) {
        PrefabUtility.InstantiatePrefab(slotPrefab, container);
      }
    }
  }
#endregion

  private void SetupSlots() {
    invSlots = invSlotsContainer.GetComponentsInChildren<InventorySlot>();
    equipSlots = equipSlotsContainer.GetComponentsInChildren<InventorySlot>();

    for (int i = 0; i < equipSlots.Length; i++) {
      equipSlots[i].defaultSprite = equipSlotsIcons[i];
    }
  }


  private void ToggleInventory() => ToggleInventory(!inventoryOpen);
  private void ToggleInventory(bool open) {
    inventoryOpen = open;
    foreach (Transform child in transform) {
      child.gameObject.SetActive(inventoryOpen);
    }
  }

  private void UpdateUI() {
    for (int i = 0; i < invSlots.Length; i++) {
      if (i < inventory.items.Count)
        invSlots[i].SetItem(inventory.items[i]);
      else
        invSlots[i].ClearSlot();
    }

    for (int i = 0; i < equipSlots.Length; i++) {
      Item item = equipment.items[i];
      if(item)
        equipSlots[i].SetItem(item);
      else
        equipSlots[i].ClearSlot();
    }
  }
}
