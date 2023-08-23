using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    int InteractTime = 0;
    protected SpriteRenderer spriteRenderer;
    private const float fadeAmount = 0.8f;
    // public const float highlightSize = 1.1f;
    private float originalAlpha;
    // private Vector2 originalSize;
    private bool isHighlighted = false;
    
    protected virtual void Awake() {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) Debug.Log("SpriteRenderer not found on " + gameObject.name);
        // Check collider
        if (gameObject.GetComponent<Collider2D>() == null) Debug.Log("Collider2D not found on " + gameObject.name);
        originalAlpha = spriteRenderer.color.a;
    }

    // TODO: ugly
    private void FixedUpdate() {
        if (isHighlighted) {
            Highlight(false);
            isHighlighted = false;
        }
    }
    public virtual void Interact()
    {
        InteractTime++;
    }

    virtual public void Highlight(bool highlight = true) {
        if (spriteRenderer == null) {
            Debug.Log("No SpriteRenderer found on " + gameObject.name);
        } else if (highlight) {
            // spriteRenderer.size = originalSize * highlightSize;
            Color highlightColor = spriteRenderer.color;
            highlightColor.a = originalAlpha*fadeAmount;
            spriteRenderer.color  = highlightColor;
            isHighlighted = true;
        } else {
            // spriteRenderer.size = originalSize;
            Color originalColor = spriteRenderer.color;
            originalColor.a = originalAlpha;
            spriteRenderer.color = originalColor;
        }
    }
}
