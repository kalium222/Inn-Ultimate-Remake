using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Openable : Interactable, IGameObjectStateHandler
{
    public Sprite sprite_Opened;
    public Sprite sprite_Closed;
    // handle in inspector
    public bool isOpened;
    public bool isEmpty;
    public GameObject content;

    // class for saved state
    class OpenableState : GameObjectStateManager.GameObjectState {
        public bool isOpened;
        public bool isEmpty;
        public OpenableState(bool isOpened, bool isEmpty) {
            this.isOpened = isOpened;
            this.isEmpty = isEmpty;
        }
    }

    override protected void Awake() {
        base.Awake();
        if (content == null) throw new System.Exception("Content not found on " + gameObject.name);
    }
    
    private void Start() {
        GameObjectStateManager.OnSave += SavetoManager;
        GameObjectStateManager.OnLoad += LoadfromManager;
        // check content
        if (!isEmpty) {
            if (transform.Find(content.name)==null) {
                throw new System.Exception("Openable: "+gameObject.name+" does not have a child");
            }
        }
        content = transform.Find(content.name).gameObject;
        SetSprite();
        SetContent();
    }

    private void OnDestroy() {
        GameObjectStateManager.OnSave -= SavetoManager;
        GameObjectStateManager.OnLoad -= LoadfromManager;
    }

    public override void Interact() {
        isOpened = !isOpened;
        SetSprite();
        SetContent();
    }

    private void SetContent(){
        if (isOpened && !isEmpty) {
            content.SetActive(true);
        } else {
            content.SetActive(false);
        }
    }

    private void SetSprite() {
        if (isOpened) {
            spriteRenderer.sprite = sprite_Opened;
            if (!isEmpty) {
                spriteRenderer.sortingOrder = -1;
            }
        } else {
            spriteRenderer.sprite = sprite_Closed;
            spriteRenderer.sortingOrder = 0;
        }
    }

    public void SavetoManager() {
        GameManager.instance.gameObjectStateManager.Add(name, new OpenableState(isOpened, isEmpty));
    }

    public void LoadfromManager() {
        if (GameManager.instance.gameObjectStateManager.Contains(name)) {
            OpenableState openableState = (OpenableState)GameManager.instance.gameObjectStateManager.Get(name);
            isOpened = openableState.isOpened;
            isEmpty = openableState.isEmpty;
        }
        SetContent();
        SetSprite();
    }

}
