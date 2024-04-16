using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BookUI : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private static readonly float enlargeFactor = 1.2f;
    private RectTransform rectTransform;
    private Image bookimage;

    // When ending draging the UI, invoking the event
    // set the position of book to the nearest position of room
    // update the doormanager
    public delegate void BookEndDragHandler(GameObject book);
    public static event BookEndDragHandler OnBookEndDrag;


    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        bookimage = GetComponent<Image>();
        if (bookimage == null) {
            Debug.LogError("Image not found!");
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        // Enlarge the book image
        bookimage.GetComponent<RectTransform>().sizeDelta *= enlargeFactor;
    }

    public void OnPointerExit(PointerEventData eventData) {
        // return the book image to normal size
        bookimage.GetComponent<RectTransform>().sizeDelta /= enlargeFactor;
    }

    public void OnDrag(PointerEventData eventData) {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData) {
        OnBookEndDrag?.Invoke(gameObject);
    }
}