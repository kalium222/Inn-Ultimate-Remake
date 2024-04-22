using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Door : Interactable, IGameObjectStateHandler, IAttackableHandler
{
    protected Portal portal;
    public bool isSealed;
    public Sprite doorSprite;
    public Sprite sealedDoorSprite;

    // class for saved state
    class DoorState : GameObjectStateManager.GameObjectState {
        public bool isSealed;
        public DoorState(bool isSealed) {
            this.isSealed = isSealed;
        }
    }

    protected override void Awake() {
        base.Awake();
        portal = GetComponent<Portal>();
        if (portal == null) throw new System.Exception("Portal not found on " + gameObject.name);
    }
    
    private void Start() {
        LoadTarget();
        SetSprite();
        PortalBookshelfUI.OnUpdatingDoor += LoadTarget;
        GameObjectStateManager.OnSave += SavetoManager;
        GameObjectStateManager.OnLoad += LoadfromManager;
    }

    private void OnDestroy() {
        PortalBookshelfUI.OnUpdatingDoor -= LoadTarget;
        GameObjectStateManager.OnSave -= SavetoManager;
        GameObjectStateManager.OnLoad -= LoadfromManager;
    }

    virtual protected void LoadTarget() {
         try {
            portal.TargetScene = GameManager.Instance.doorManager.GetTargetScene(gameObject.name);
            portal.TargetPortal = GameManager.Instance.doorManager.GetTargetDoor(gameObject.name);
        } catch (KeyNotFoundException) {
            portal.TargetScene = "Room0";
            portal.TargetPortal = "Room0Door";
            Debug.LogError("Door " + gameObject.name + " not found in doorRoomBijection");
            throw;
        }
    }

    private void SetSprite() {
        if (isSealed) {
            GetComponent<SpriteRenderer>().sprite = sealedDoorSprite;
        } else {
            GetComponent<SpriteRenderer>().sprite = doorSprite;
        }
    }

    public override void Interact()
    {
        if (isSealed) {
            // TODO: animation?
            GameUIManager.instance.ShowDialogue("The door is sealed.");
            return;
        }
        portal.Teleport();
    }

    public void SavetoManager() {
        GameManager.Instance.gameObjectStateManager.Add(name, new DoorState(isSealed));
    }

    public void LoadfromManager() {
        if (GameManager.Instance.gameObjectStateManager.Contains(name)) {
            DoorState doorState = (DoorState)GameManager.Instance.gameObjectStateManager.Get(name);
            isSealed = doorState.isSealed;
        }
        SetSprite();
    }

    virtual public void OnAttack(in MeleeAttack meleeAttack) {
        if (meleeAttack.kind == MeleeAttack.MeleeAttackKind.blow) {
            isSealed = false;
            GetComponent<SpriteRenderer>().sprite = doorSprite;
        }
    }

}
