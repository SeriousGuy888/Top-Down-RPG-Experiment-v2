using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// [ExecuteInEditMode]
public class InventorySlot : MonoBehaviour {
  public Sprite defaultSprite; // Sprite to display as icon when slot is empty

  public Image itemImage; // The image component in which to display the icon
  public Image borderImage; // The image component that is visible when this slot is selected
  public TMP_Text quantityText; // The text component for stack number

  private bool hasItem = false;

  public event Action<InventorySlot>
    OnItemClicked,
    OnItemRightClicked,
    OnItemDroppedOn, // When dropping item on another slot
    OnItemDragStart,
    OnItemDragEnd;


  private void Awake() {
    ResetData();
    Deselect();
  }

  public void ResetData() {
    itemImage.gameObject.SetActive(false);
    hasItem = false;
  }

  public void SetData(Sprite sprite, int quantity) {
    itemImage.gameObject.SetActive(true);
    itemImage.sprite = sprite;
    quantityText.text = quantity.ToString();
    hasItem = true;
  }

  private void Deselect() {
    borderImage.enabled = false;
  }

  public void Select() {
    borderImage.enabled = true;
  }



  public void OnDragStart() {
    if (hasItem)
      OnItemDragStart?.Invoke(this);
  }

  public void OnDragEnd() {
    OnItemDragEnd?.Invoke(this);
  }

  public void OnDrop() {
    OnItemDroppedOn.Invoke(this);
  }

  public void OnPointerClick(BaseEventData data) {
    if (!hasItem)
      return;

    var pointerData = (PointerEventData)data;
    if (pointerData.button == PointerEventData.InputButton.Right)
      OnItemRightClicked?.Invoke(this);
    else
      OnItemClicked?.Invoke(this);
  }



  // private void Update() {
  //   if (Application.isPlaying)
  //     return;

  //   if (item == null)
  //     SetEmptyIcon();
  // }

  // public void SetItem(Item newItem) {
  //   item = newItem;

  //   itemImage.sprite = item.icon;
  //   itemImage.color = new Color(1, 1, 1, 1);
  //   itemImage.enabled = true;
  // }

  // public void ClearSlot() {
  //   item = null;
  //   SetEmptyIcon();
  // }

  // private void SetEmptyIcon() {
  //   if (defaultSprite != null) {
  //     itemImage.sprite = defaultSprite;
  //     itemImage.color = new Color(1, 1, 1, 0.5f);
  //     itemImage.enabled = true;
  //   } else {
  //     itemImage.sprite = null;
  //     itemImage.enabled = false;
  //   }
  // }

  // public void UseItem() {
  //   if (item == null)
  //     return;

  //   item.Use();
  // }
}
