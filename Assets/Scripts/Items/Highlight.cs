using UnityEngine;

public class Highlight : CommonBehaviourBase {
    private SpriteRenderer m_spriteRenderer;
    private const float k_originalAlpha = 1f;
    private const float k_fadeFactor = 0.8f;
    public bool isHighlighted { get; set; }

    public Highlight(GameObject gameObject, SpriteRenderer spriteRenderer)
    : base(gameObject) {
        isHighlighted = false;
        m_spriteRenderer = spriteRenderer;
    }

    public void EnableHighlight() {
        Color highlightColor = m_spriteRenderer.color;
        highlightColor.a = k_originalAlpha * k_fadeFactor;
        m_spriteRenderer.color = highlightColor;
    }
    
    public void DisableHighlight() {
        Color originalColor = m_spriteRenderer.color;
        originalColor.a = k_originalAlpha;
        m_spriteRenderer.color = originalColor;
    }
}
