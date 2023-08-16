using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collectable : Interactable, IGameObjectStateHandler
{
    public bool isCollected = false;
    
    protected override void Awake() {
        base.Awake();
        isCollected = false;
    }

    private void Start() {
        // GameManager.instance.collectableManager.changedCollectableInfos.Add(new CollectableInfo(SceneManager.GetActiveScene().name, transform, gameObject));
        // transform.SetParent(GameManager.instance.transform);
        GameObjectStateManager.OnSave += SavetoManager;
        GameObjectStateManager.OnLoad += LoadfromManager;
    }

    private void OnDestroy() {
        GameObjectStateManager.OnSave -= SavetoManager;
        GameObjectStateManager.OnLoad -= LoadfromManager;
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
        HeroInteraction.instance.Bag.Add(gameObject);
        GameManager.instance.collectableManager.changedCollectableInfos.Add(new CollectableInfo(SceneManager.GetActiveScene().name, transform, gameObject));
        transform.SetParent(GameManager.instance.transform);
        gameObject.SetActive(false);
        isCollected = true;
    }

    public void Drop() {
        HeroInteraction.instance.Bag.Remove(gameObject);
        gameObject.SetActive(true);
        GameManager.instance.collectableManager.setChangedCollectableInfos(new CollectableInfo(SceneManager.GetActiveScene().name, transform, gameObject));
        gameObject.transform.position = HeroController.instance.transform.position;
        gameObject.transform.SetParent(GameManager.instance.transform);
        isCollected = false;
    }

    virtual public void Use() {
        Drop();
    }

    public void SavetoManager() {
        // Debug.Log("SavetoManager: " + gameObject.name);
    }

    public void LoadfromManager() {
        // Debug.Log("LoadfromManager: " + gameObject.name);
    }
}
