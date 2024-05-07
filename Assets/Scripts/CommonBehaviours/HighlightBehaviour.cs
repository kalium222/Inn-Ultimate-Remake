using UnityEngine;

public class HighlightBehaviour : CommonBehaviourBase {
    private SpriteRenderer m_spriteRenderer;
    private const float k_originalAlpha = 1f;
    private const float k_fadeFactor = 0.6f;
    public bool isHighlighted { get; set; }

    public HighlightBehaviour(GameObject gameObject, SpriteRenderer spriteRenderer)
    : base(gameObject) {
        isHighlighted = false;
        m_spriteRenderer = spriteRenderer;
    }

    public void EnableHighlight() {
        Color highlightColor = m_spriteRenderer.color;
        highlightColor.a = k_originalAlpha * k_fadeFactor;
        m_spriteRenderer.color = highlightColor;
#if SCRIPT_TEST
        Debug.Log("Enable highlight " + gameObject.name);
#endif
    }
    
    public void DisableHighlight() {
        Color originalColor = m_spriteRenderer.color;
        originalColor.a = k_originalAlpha;
        m_spriteRenderer.color = originalColor;
#if SCRIPT_TEST
        Debug.Log("Disable highlight " + gameObject.name);
#endif
    }
}

