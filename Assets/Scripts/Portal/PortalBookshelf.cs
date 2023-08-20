using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBookshelf : Talkable
{
    private const string portalController = "PortalControllerUI";
    GameObject portalBookshelfController;

    protected override void Start() {
        base.Start();
        portalBookshelfController = GameObject.Find(portalController);
        if (portalBookshelfController == null) {
            Debug.LogError("PortalBookshelfUI not found");
        }
        if (portalBookshelfController.GetComponent<PortalBookshelfUI>() == null) {
            Debug.LogError("PortalBookshelfUI script lost");
        }
        portalBookshelfController.SetActive(false);
    }

    protected override void DialogManagerInit() {
        base.dialogManager = new PortalBookshelfDialogManager();
        dialogManager.SetNormalConversations(new List<Dialogue>(){
            new Dialogue(true, "Better not to touch it while its owner is here."),
            // new Dialogue(false, "There is nobody else here.")
        });
        dialogManager.SetSpecialDialogue(new Dialogue(false, "There is nobodyelse here. Check the bookshelf?"));
    }

    protected override void DoSpecialEvent() {
        GameUIManager.instance.ClearDialogBox();
        portalBookshelfController.GetComponent<PortalBookshelfUI>().AccessBookshelf();
    }

    //---------------------------Subclass for dialogmanager of PortalBookshelf-------------------
    private class PortalBookshelfDialogManager : DialogManager {
        public override bool IsSpecialConversation() {
            return GameManager.instance.gameStageManager.CurrentStage == GameStageManager.Stage.Exploring;
        }
    }
}
