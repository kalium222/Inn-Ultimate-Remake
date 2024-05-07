using UnityEngine;

// TODO: make this replace the original one
public class HeroInteractController : MonoBehaviour {
    private InteractBehaviour m_heroInteract;
    [SerializeField]
    private Control control;

    private void Awake() {
        m_heroInteract = new(gameObject);
    }

    private void Start() {
        control = GameManager.Instance.Control;
        control.gameplay.SelectNext.performed += m_heroInteract.OnNextSelected;
        control.gameplay.Interact.performed += m_heroInteract.Interact;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        m_heroInteract.OnReachItem(other);
    }

    private void OnTriggerExit2D(Collider2D other) {
        m_heroInteract.OnLeaveItem(other);
    }

}

