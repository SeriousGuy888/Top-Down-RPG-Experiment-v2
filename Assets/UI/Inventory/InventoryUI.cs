using System.Net.Sockets;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class InventoryUI : MonoBehaviour {
  public Inventory inventory;
  public Equipment equipment;

  public GameObject slotPrefab;

  public Transform invSlotsContainer;
  private InventorySlot[] invSlots;

  public Transform equipSlotsContainer;
  public Sprite[] equipSlotsIcons;
  private InventorySlot[] equipSlots;

  private InputMaster controls;

  private void Start() {
    if (!Application.isPlaying)
      return;

    controls = GameManager.Instance.controls;
    controls.Player.ToggleInventory.performed += _ => ToggleInventory();
    invSlotsContainer.gameObject.SetActive(false);

    inventory.onItemChangedCallback += UpdateUI;

    invSlots = invSlotsContainer.GetComponentsInChildren<InventorySlot>();
    equipSlots = equipSlotsContainer.GetComponentsInChildren<InventorySlot>();
  }

  private void Update() {
    if (Application.isPlaying)
      return;

    GenerateRequiredSlots(invSlotsContainer, Mathf.Max(inventory.space, 0), false);
    GenerateRequiredSlots(equipSlotsContainer, Mathf.Max(equipment.slotCount, 0), true);
  }

  private void GenerateRequiredSlots(Transform container, int neededSlots, bool useEquipmentIcons) {
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

    if (useEquipmentIcons) {
      for (int i = 0; i < neededSlots; i++) {
        if (i < equipSlotsIcons.Length)
          container.GetChild(i).GetComponent<InventorySlot>().defaultSprite = equipSlotsIcons[i];
      }
    }
  }

  private void ToggleInventory() {
    invSlotsContainer.gameObject.SetActive(!invSlotsContainer.gameObject.activeSelf);
  }

  private void UpdateUI() {
    for (int i = 0; i < invSlots.Length; i++) {
      if (!invSlots[i].gameObject.activeSelf) {
        i--;
        continue;
      }

      if (i < inventory.items.Count)
        invSlots[i].SetItem(inventory.items[i]);
      else
        invSlots[i].ClearSlot();
    }
  }
}
