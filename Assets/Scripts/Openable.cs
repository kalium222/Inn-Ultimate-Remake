using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Openable : Interactable
{
    public Sprite sprite_Opened;
    public Sprite sprite_Closed;
    public bool isOpened = false;
    public bool isEmpty = false;
    public GameObject content;        
    
    private void Start() {
        if (!GameManager.instance.openableManager.openableInfos.ContainsKey(gameObject.name)) {
            Debug.LogError("Openable: "+gameObject.name+" is not found");
           return;
        }
        isOpened = GameManager.instance.openableManager.openableInfos[gameObject.name].isOpened;
        isEmpty = GameManager.instance.openableManager.openableInfos[gameObject.name].isEmpty;
        if (!isEmpty) {
            if (transform.Find(content.name)==null) {
                Debug.LogError("Openable: "+gameObject.name+" does not have a child");
            }
        }
        content = transform.Find(content.name).gameObject;
        content.SetActive(false);
        base.spriteRenderer = GetComponent<SpriteRenderer>();
        setSprite();
        setContent();
    }
    public override void Interact() {
        isOpened = !isOpened;
        setSprite();
        setContent();
        GameManager.instance.openableManager.setValue(gameObject.name, isOpened, isEmpty);
    }

    private void setContent(){
        if (isOpened&&!isEmpty) {
            content.SetActive(true);
        } else if (!isOpened&&!isEmpty) {
            content.SetActive(false);
        }
    }

    private void setSprite() {
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
}
