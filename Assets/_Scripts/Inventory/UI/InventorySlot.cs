using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteInEditMode]
public class InventorySlot : MonoBehaviour, IPointerClickHandler, IDropHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {
  public Sprite defaultSprite; // Sprite to display as icon when slot is empty

  public Image itemImage; // The image component in which to display the icon
  public Image borderImage; // The image component that is visible when this slot is selected
  public GameObject quantityTextBg; // The background image object for quantity text so that it is readable. 
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

  private void Update() {
    if (!Application.isPlaying)
      ResetData();
  }

  public void ResetData() {
    if (defaultSprite != null) {
      itemImage.gameObject.SetActive(true);
      itemImage.sprite = defaultSprite;
      itemImage.color = new Color(1, 1, 1, 0.5f);
    } else {
      itemImage.gameObject.SetActive(false);
    }
    quantityTextBg.SetActive(false);
    hasItem = false;
  }

  public void SetData(Sprite sprite, int quantity) {
    itemImage.gameObject.SetActive(true);
    itemImage.sprite = sprite;
    itemImage.color = new Color(1, 1, 1, 1);

    quantityText.text = quantity.ToString();
    hasItem = true;

    if (quantity != 1) {
      quantityTextBg.SetActive(true);
    }
  }

  public void Deselect() {
    borderImage.enabled = false;
  }

  public void Select() {
    borderImage.enabled = true;
  }



  public void OnPointerClick(PointerEventData pointerData) {
    if (pointerData.button == PointerEventData.InputButton.Right)
      OnItemRightClicked?.Invoke(this);
    else
      OnItemClicked?.Invoke(this);
  }

  public void OnDrop(PointerEventData eventData) {
    OnItemDroppedOn.Invoke(this);
  }

  public void OnBeginDrag(PointerEventData eventData) {
    if (hasItem)
      OnItemDragStart?.Invoke(this);
  }

  public void OnEndDrag(PointerEventData eventData) {
    OnItemDragEnd?.Invoke(this);
  }

  public void OnDrag(PointerEventData eventData) {
    // IDragHandler is required for begin and end drag events.
    // This method is intentionally left empty.
  }
}
