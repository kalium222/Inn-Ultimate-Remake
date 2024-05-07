using UnityEngine;
using Utils;

public class ItemController : MonoBehaviour, IInteractable {
    private HighlightBehaviour m_highlight;
    public void OnRefreshHighlight(GameObject other) {
        // TODO:
        if (gameObject==other)
            m_highlight.EnableHighlight();
        else 
            m_highlight.DisableHighlight();
    }

    protected virtual void Awake() {
        this.GetAndCheckComponent(out SpriteRenderer spriteRenderer);
        m_highlight = new(gameObject, spriteRenderer);
    }

    public void Interact() {
        // TODO:
#if SCRIPT_TEST
        Debug.Log("Interact with " + name);
#endif
    }
}

