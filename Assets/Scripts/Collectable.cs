using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collectable : Interactable, IGameObjectStateHandler
{
    public bool isCollected = false;
    // handle in inspector, for example, "RawBigFood"
    public string kind;

    // class for saved state
    class CollectableState : GameObjectStateManager.GameObjectState {
        public bool isCollected;
        public Vector3 position;
        public string sceneName;
        public CollectableState(bool isCollected, Vector3 position, string sceneName) {
            this.isCollected = isCollected;
            this.position = position;
            this.sceneName = sceneName;
        }
    }
    
    protected override void Awake() {
        base.Awake();
        isCollected = false;
    }

    protected virtual void Start() {
        if (GameManager.instance.gameObjectStateManager.Contains(gameObject.name)) {
            Destroy(gameObject);
            return;
        }
        GameObjectStateManager.OnSave += SavetoManager;
        GameObjectStateManager.OnLoad += LoadfromManager;
    }

    private void OnDestroy() {
        GameObjectStateManager.OnSave -= SavetoManager;
        GameObjectStateManager.OnLoad -= LoadfromManager;
    }

    // if isCollected, set the gameObject to inactive
    private void SetObject() {
        gameObject.SetActive(!isCollected);
    }

    
    // TODO: ugly
    // Interact of collectable is taking it
    // put the item in bag and put the gameobject in GameManager.instance.collectableManager.changedCollectableInfos
    public override void Interact()
    {
        // If the collectable is got from a openable, set the openable to empty
        if (transform.parent!=null && transform.parent.GetComponent<Openable>()!=null) {
            transform.parent.GetComponent<Openable>().isEmpty = true;
        }
        HeroInteraction.instance.bag.Add(gameObject);
        transform.position = HeroController.instance.transform.position;
        transform.SetParent(HeroController.instance.transform);
        isCollected = true;
        SetObject();
    }

    public void Drop() {
        HeroInteraction.instance.bag.Remove(gameObject);
        gameObject.transform.position = HeroController.instance.transform.position;
        gameObject.transform.SetParent(GameManager.instance.transform);
        isCollected = false;
        SetObject();
        // TODO: ugly
        HeroInteraction.instance.SetAnimation();
    }

    virtual public void Use() {
        Drop();
    }

    // TODO: bit of ugly
    public void SavetoManager() {
        transform.SetParent(GameManager.instance.transform);
        if (isCollected) {
            GameManager.instance.gameObjectStateManager.Add(gameObject.name, new CollectableState(isCollected, transform.position, "InBag"));
            // Debug.Log("SavetoManager: " + gameObject.name);
            // Debug.Log("isCollected: " + isCollected + ", In bag");
        } else if (gameObject.activeSelf) {
            GameManager.instance.gameObjectStateManager.Add(gameObject.name, new CollectableState(isCollected, transform.position, SceneManager.GetActiveScene().name));
            // Debug.Log("SavetoManager: " + gameObject.name);
            // Debug.Log("isCollected: " + isCollected + ", SceneName: " + SceneManager.GetActiveScene().name);
        }
    }

    public void LoadfromManager() {
        // Debug.Log("LoadfromManager: " + gameObject.name);
        CollectableState state = (CollectableState)GameManager.instance.gameObjectStateManager.Get(gameObject.name);
        if (state == null) return;
        isCollected = state.isCollected;
        transform.position = state.position;
        // Debug.Log("isCollected: " + isCollected + ", SceneName: " + state.sceneName + ", ActiveSceneName: " + SceneManager.GetActiveScene().name);

        if (state.sceneName != SceneManager.GetActiveScene().name) {
            gameObject.SetActive(false);
        } else SetObject();
    }
}
