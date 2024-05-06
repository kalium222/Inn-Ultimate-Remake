using UnityEngine;
using Utils;

public class ItemController : MonoBehaviour, IInteractable {
    private Highlight m_highlight;

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

