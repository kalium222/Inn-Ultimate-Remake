using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroInteraction : MonoBehaviour
{
    public static HeroInteraction instance;

    // handle in inspector
    public float interactRadius = 0.5f;
    public KeyCode switchObjectKey = KeyCode.K;
    public KeyCode interactKey = KeyCode.F;
    public KeyCode useKey = KeyCode.J;
    public KeyCode nextItemKey = KeyCode.E;
    public KeyCode previousItemKey = KeyCode.Q;
    public LayerMask interactionLayer = 6;

    // state parameter deciding whether the hero can interact with objects
    private bool canInteract = true;
    public bool CanInteract {
        get { return canInteract; }
        set { canInteract = value; }
    }
    private Animator animator;
    private GameUIManager gameUIManager;
    // list of objects that can be interacted with
    private Collider2D[] interactableObjectColliders;
    private int currentObjectIndex = 0;
    // bag
    public Bag bag = new Bag();

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        animator = GetComponent<Animator>();
        if (animator == null) throw new System.Exception("Animator not found on " + gameObject.name);
    }

    private void Start() {
        gameUIManager = GameUIManager.instance;
        if (gameUIManager == null) throw new System.Exception("GameUIManager not found");
    }

    // TODO: so ugly
    private void Update() {
        if (!canInteract) return;
        interactableObjectColliders = Physics2D.OverlapCircleAll(transform.position, interactRadius, interactionLayer);
        if (interactableObjectColliders.Length > 0) {
            HighlightCurrentInteractable();
            GameUIManager.instance.Interact.SetActive(true);
            if (Input.GetKeyDown(switchObjectKey)) {
                currentObjectIndex = (currentObjectIndex + 1) % interactableObjectColliders.Length;
            }
            if (Input.GetKeyDown(interactKey)) {
                interactableObjectColliders[currentObjectIndex % interactableObjectColliders.Length].GetComponent<Interactable>().Interact();
                SetAnimation();
            }
            if (interactableObjectColliders.Length>1) {
                GameUIManager.instance.Exchange.SetActive(true);
            } else {
                GameUIManager.instance.Exchange.SetActive(false);
            }
        } else {
            GameUIManager.instance.Interact.SetActive(false);
        }
        if (Input.GetKeyDown(useKey)) {
            UseCurrentItem();
            SetAnimation();
        }
        if (Input.GetKeyDown(nextItemKey)) {
            bag.ItemsiteratorNext();
            SetAnimation();
        }
        if (Input.GetKeyDown(previousItemKey)) {
            bag.ItemsiteratorPrevious();
            SetAnimation();
        }

        // Test code
        if (Input.GetKeyDown(KeyCode.B)) {
            bag.Print();
        }
        // End of test code
    }

    // A trigger function for animation
    public void SetAnimation() {
        animator.SetBool("Holding", bag.GetCurrentItemKind()!="emptyhanded");
    }

    private void HighlightCurrentInteractable() {
        GameObject curr = interactableObjectColliders[currentObjectIndex%interactableObjectColliders.Length].gameObject;
        if (curr.GetComponent<Interactable>() == null) {
            return;
            // Debug.LogError("No Interactable component found on " + curr.name);
        } else {
            curr.GetComponent<Interactable>().Highlight();
        }
    }

    public void UseCurrentItem() {
        GameObject currentItem = bag.GetCurrentItem();
        currentItem?.GetComponent<Collectable>()?.Use();
    }

    // ---------------------------------Tool class for bag
    public class Bag {
        private int itemsiterator = 0;
        private List<GameObject> items = new List<GameObject>(){null};
        
        public GameObject GetCurrentItem() {
            return items[itemsiterator];
        }

        public string GetCurrentItemKind() {
            if (GetCurrentItem() == null) return "emptyhanded";
            return GetCurrentItem().GetComponent<Collectable>().kind;
        }
        public void ItemsiteratorNext() {
            itemsiterator = (itemsiterator + 1) % items.Count;
        }
        public void ItemsiteratorPrevious() {
            itemsiterator = (itemsiterator - 1 + items.Count) % items.Count;
        }
        
        public void Add(GameObject item) {
            if (item.GetComponent<Collectable>() == null) {
                Debug.LogError("No Collectable component found on " + item.name);
                return;
            }
            if (items.Contains(item)) {
                Debug.Log(item.name + " already in bag");
            } else {
                items.Add(item);
                itemsiterator = items.Count - 1;
            }
        }

        public void Remove() {
            if (GetCurrentItem() == null) return;
            Remove(items[itemsiterator]);
            itemsiterator = 0;
        }

        public void Remove(GameObject item) {
            if (item == null) return;
            if (!items.Contains(item)) {
                Debug.Log(item.name + " not found in bag");
                return;
            }
            items.Remove(item);
            itemsiterator = 0;
        }

        // --------------------------Test function for bag
        public void Print() {
            Debug.Log("Iterator: " + itemsiterator);
            Debug.Log("Current item is " + items[itemsiterator].name);
            Debug.Log("Bag contains:");
            foreach (GameObject item in items) {
                Debug.Log(item.name);
            }
            if (items.Count > 0) {
                Debug.Log("Current item is " + items[itemsiterator].name);
            }   
        }
    }
}