using UnityEngine;

// TODO: make this replace the original one
public class HeroInteract : MonoBehaviour {
    private InteractController m_heroInteractController;
    [SerializeField]
    private Control control;

    private void Awake() {
        m_heroInteractController = new(gameObject);
    }

    private void Start() {
        control = GameManager.Instance.Control;
        control.gameplay.SelectNext.performed += m_heroInteractController.OnNextSelected;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        m_heroInteractController.OnReachItem(other);
    }

    private void OnTriggerExit2D(Collider2D other) {
        m_heroInteractController.OnLeaveItem(other);
    }

}

