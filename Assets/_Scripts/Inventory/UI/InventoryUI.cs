using System;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class InventoryUI : MonoBehaviour {
  [Header("Controller Scripts")]
  public Inventory inventory;
  public Equipment equipment;

  [Header("Objects")]
  public Transform inventorySlotsContainer;
  public Transform equipmentSlotsContainer;
  public MouseFollower mouseFollower;
  public InventoryDescription descriptionPanel;

  [Header("Prefabs & Sprites")]
  public InventorySlot slotPrefab;
  public Sprite[] equipmentSlotSprites;

  private InventorySlot[] inventorySlots;
  private int currentDraggedItemIndex = -1;

  public event Action<int>
    OnDescriptionRequested,
    OnItemActionsRequested,
    OnDragStart;
  public event Action<int, int>
    OnSwapItems;


  private void Awake() {
    if (!Application.isPlaying)
      return;

    Hide();
    mouseFollower.Toggle(false);
    descriptionPanel.ResetDescription();
  }

  private void Update() {
    if (!Application.isPlaying)
      InitInventoryUI();
  }

  public void Show() {
    gameObject.SetActive(true);
    ResetSelection();
  }
  public void Hide() {
    gameObject.SetActive(false);
    ResetDraggedItem();
  }

  public void InitInventoryUI() {
    inventorySlots = new InventorySlot[inventory.mainSlotCount + inventory.equipmentSlotCount];

    var mainSlots = GenerateSlots(inventorySlotsContainer, inventory.mainSlotCount);
    var equipmentSlots = GenerateSlots(equipmentSlotsContainer, inventory.equipmentSlotCount);

    for (int i = 0; i < mainSlots.Length; i++) {
      inventorySlots[i] = mainSlots[i];
    }

    inventory.reservedEquipmentSlots = new();
    for (int i = 0; i < equipmentSlots.Length; i++) {
      inventorySlots[inventory.mainSlotCount + i] = equipmentSlots[i];
      inventory.reservedEquipmentSlots[i] = (AssignedEquipmentSlot)i;

      var slot = equipmentSlots[i];
      slot.defaultSprite = equipmentSlotSprites[i]; // set default sprite to appropriate equipment icon
      slot.ResetData(); // make sure it renders its default sprite
    }

    if (equipmentSlotSprites.Length != equipmentSlotsContainer.childCount)
      Debug.LogWarning("Number of equipment slot sprites != number of equipment slots");


    if (Application.isPlaying) {
      foreach (var slot in inventorySlots) {
        slot.OnItemClicked += HandleItemSelect;
        slot.OnItemRightClicked += HandleItemShowActions;
        slot.OnItemDragStart += HandleDragStart;
        slot.OnItemDragEnd += HandleDragEnd;
        slot.OnItemDroppedOn += HandleSwap;
      }
    }
  }

  private InventorySlot[] GenerateSlots(Transform container, int neededSlots) {
    neededSlots = Math.Max(neededSlots, 0);
    int existingSlots = container.childCount;

    if (existingSlots < neededSlots) {
      for (int i = 0; i < neededSlots - existingSlots; i++)
        PrefabUtility.InstantiatePrefab(slotPrefab, container);
    }
    while (existingSlots > neededSlots) {
      DestroyImmediate(container.GetChild(neededSlots).gameObject);
      existingSlots--;
    }

    var slots = new InventorySlot[neededSlots];
    for (int i = 0; i < neededSlots; i++)
      slots[i] = container.GetChild(i).GetComponent<InventorySlot>();
    return slots;
  }


  public void SetSlotData(int index, Sprite icon, int quantity) {
    if (inventorySlots.Length > index) {
      inventorySlots[index].SetData(icon, quantity);
    }
  }
  public void SetDescription(Sprite icon, string title, string description) {
    descriptionPanel.SetDescription(icon, title, description);
  }


  private void HandleItemSelect(InventorySlot slot) {
    int index = Array.IndexOf(inventorySlots, slot);
    if (index == -1)
      return;

    ResetSelection();
    slot.Select();

    OnDescriptionRequested?.Invoke(index);
  }

  private void HandleItemShowActions(InventorySlot slot) {
    int index = Array.IndexOf(inventorySlots, slot);
    if (index == -1)
      return;
    OnItemActionsRequested?.Invoke(index);
  }

  private void HandleDragStart(InventorySlot slot) {
    int index = Array.IndexOf(inventorySlots, slot);
    if (index == -1)
      return;
    currentDraggedItemIndex = index;

    HandleItemSelect(slot); // select the slot that is being dragged
    OnDragStart?.Invoke(index);
  }

  private void HandleDragEnd(InventorySlot slot) {
    ResetDraggedItem();
  }

  private void HandleSwap(InventorySlot slot) {
    int swappingIndex = Array.IndexOf(inventorySlots, slot);

    // swappingIndex can be -1 if the slot dragged TO doesn't exist
    // currentDraggedItemIndex can be -1 if the slot dragged FROM is empty
    if (swappingIndex == -1 || currentDraggedItemIndex == -1)
      return;

    OnSwapItems?.Invoke(currentDraggedItemIndex, swappingIndex);
    HandleItemSelect(slot); // select the new slot of the dragged item
  }


  public void CreateDraggedItem(Sprite sprite, int quantity) {
    mouseFollower.Toggle(true);
    mouseFollower.SetData(sprite, quantity);
  }

  public void ResetDraggedItem() {
    mouseFollower.Toggle(false);
    currentDraggedItemIndex = -1;
  }

  private void ResetSelection() {
    descriptionPanel.ResetDescription();
    foreach (var slot in inventorySlots) {
      slot.Deselect();
    }
  }

  public void ResetSlots() {
    ResetSelection();
    foreach (var slot in inventorySlots) {
      slot.ResetData();
    }
  }
}
