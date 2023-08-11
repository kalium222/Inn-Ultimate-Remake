using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public Portal portal;
    public bool isSealed;
    public Sprite doorSprite;
    public Sprite sealedDoorSprite;
    private void Start() {
        portal = GetComponent<Portal>();
        LoadTarget();
        if (isSealed) {
            GetComponent<SpriteRenderer>().sprite = sealedDoorSprite;
        } else {
            GetComponent<SpriteRenderer>().sprite = doorSprite;
        }
    }

    public void LoadTarget() {
         try {
            portal.TargetScene = GameManager.instance.doorManager.GetTargetScene(gameObject.name);
            portal.TargetPortal = GameManager.instance.doorManager.GetTargetDoor(gameObject.name);
        } catch (KeyNotFoundException) {
            portal.TargetScene = "Room0";
            portal.TargetPortal = "Room0Door";
            Debug.LogError("Door " + gameObject.name + " not found in doorRoomBijection");
            throw;
        }
    }

    public override void Interact()
    {
        if (isSealed) {
            Debug.Log("TODO: Sealed Door");
            return;
        }
        portal.Teleport();
    }
    
}
