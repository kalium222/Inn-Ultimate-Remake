using UnityEngine;
using Utils;

public class ItemController : MonoBehaviour, IInteractable {
    private HighlightBehaviour m_highlight;
    public void OnRefreshHighlight(GameObject other) {
        if (gameObject==other)
            m_highlight.EnableHighlight();
        else 
            m_highlight.DisableHighlight();
    }

    protected virtual void Awake() {
        this.GetAndCheckComponent(out SpriteRenderer spriteRenderer);
        m_highlight = new(gameObject, spriteRenderer);
    }

    public virtual void Interact() {
#if SCRIPT_TEST_HIGHLIGHT
        Debug.Log("Interact with " + name);
#endif
    }
}

