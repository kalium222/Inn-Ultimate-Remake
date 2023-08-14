using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroInteraction : MonoBehaviour
{
    // TODO: So ugly
    public static HeroInteraction instance;
    public float interactRadius = 0.5f;
    public KeyCode switchObjectKey = KeyCode.K;
    public KeyCode interactKey = KeyCode.F;
    public KeyCode useKey = KeyCode.J;
    public KeyCode nextItemKey = KeyCode.E;
    public KeyCode previousItemKey = KeyCode.Q;
    public LayerMask interactionLayer = 6;

    private bool canInteract = true;
    private Animator animator;
    private Collider2D[] interactableObjectColliders;
    private int currentObjectIndex = 0;
    private Bag bag = new Bag();

    public bool CanInteract {
        get { return canInteract; }
        set { canInteract = value; }
    }
    public Bag Bag {
        get { return bag; }
    }

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        animator = GetComponent<Animator>();
    }

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
            bag.itemsiteratorNext();
            SetAnimation();
        }
        if (Input.GetKeyDown(previousItemKey)) {
            bag.itemsiteratorPrevious();
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
        animator.SetBool("Holding", bag.getCurrentItemName()!="emptyhanded");
    }

    private void HighlightCurrentInteractable() {
        GameObject curr = interactableObjectColliders[currentObjectIndex%interactableObjectColliders.Length].gameObject;
        if (curr.GetComponent<Interactable>() == null) {
            Debug.LogError("No Interactable component found on " + curr.name);
        } else {
            curr.GetComponent<Interactable>().Highlight();
        }
    }

    public void UseCurrentItem() {
        ItemInBag currentItem = bag.getCurrentItem();
        if (currentItem.name == "emptyhanded") return;
        foreach (CollectableInfo item in GameManager.instance.collectableManager.changedCollectableInfos) {
            if (item.collectable.name == currentItem.name) {
                if (currentItem.isUsable) {
                    item.collectable.GetComponent<Collectable>().Use();
                } else {
                    item.collectable.GetComponent<Collectable>().Drop();
                }
                return;
            }
        }
        
    }
}

public class ItemInBag {
    public string name;
    public int quantity;
    public bool isUsable = false;
    public ItemInBag(string name, int quantity, bool isUsable = false) {
        this.name = name;
        this.quantity = quantity;
        this.isUsable = isUsable;
    }
}

public class Bag {
    private int itemsiterator = 0;
    private List<ItemInBag> items = new List<ItemInBag>(){
        new ItemInBag("emptyhanded", 0)
    };

    public GameObject getCurrentItemObject() {
        return GameManager.instance.collectableManager.changedCollectableInfos.Find(x => x.collectable.name == getCurrentItemName()).collectable;
    }

    public ItemInBag getCurrentItem() {
        return items[itemsiterator];
    }

    public string getCurrentItemName() {
        return items[itemsiterator].name;
    }
    public void itemsiteratorNext() {
        itemsiterator = (itemsiterator + 1) % items.Count;
    }
    public void itemsiteratorPrevious() {
        itemsiterator = (itemsiterator - 1 + items.Count) % items.Count;
    }
    
    public void Add(ItemInBag item) {
        if (items.Contains(item)) {
            items[items.IndexOf(item)].quantity++;
        } else {
            items.Add(item);
            itemsiterator = items.Count - 1;
        }
    }

    public void Add(GameObject item) {
        if (item.GetComponent<Weapon>() != null) {
            Add(new ItemInBag(item.name, 1, true));
        } else {
            Add(new ItemInBag(item.name, 1));
        }
        
    }

    public void Remove(ItemInBag item) {
        if (item.name == "emptyhanded") return;
        for (int i = 0; i < items.Count; i++) {
            if (items[i].name == item.name) {
                if (items[i].quantity > 1) {
                    items[i].quantity--;
                } else {
                    items.Remove(items[i]);
                    itemsiterator = 0;
                }
            }
        }
    }

    public void Remove() {
        Remove(items[itemsiterator]);
    }

    public void Remove(GameObject item) {
        Remove(new ItemInBag(item.name, 1));
    }


    // ---------------------------------Test function
    public void Print() {
        Debug.Log("Iterator: " + itemsiterator);
        Debug.Log("Current item is " + items[itemsiterator].name);
        Debug.Log("Current item quantity is " + items[itemsiterator].quantity + "");
        Debug.Log("Current item is usable: " + items[itemsiterator].isUsable + "");
        Debug.Log("Bag contains:");
        foreach (ItemInBag item in items) {
            Debug.Log(item.name);
        }
        if (items.Count > 0) {
            Debug.Log("Current item is " + items[itemsiterator].name);
        }   
    }
}
