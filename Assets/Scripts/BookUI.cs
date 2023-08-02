using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BookUI : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private static float enlargeFactor = 1.2f;
    private RectTransform rectTransform;
    private Image bookimage;


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
        Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData) {
        Debug.Log("OnEndDrag");
    }
}