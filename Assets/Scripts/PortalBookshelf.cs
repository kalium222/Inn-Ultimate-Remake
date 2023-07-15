using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBookshelf : Talkable
{
    private const string portalController = "PortalControllerUI";
    GameObject portalBookshelfUI;

    protected override void Start() {
        base.Start();
        portalBookshelfUI = GameObject.Find(portalController);
        if (portalBookshelfUI == null) {
            Debug.LogError("PortalBookshelfUI not found");
        }
        if (portalBookshelfUI.GetComponent<PortalBookshelfUI>() == null) {
            Debug.LogError("PortalBookshelfUI script lost");
        }
        portalBookshelfUI.SetActive(false);
    }

    protected override void DialogManagerInit() {
        base.dialogManager = new PortalBookshelfDialogManager();
        dialogManager.setNormalConversations(new List<Conversation>(){
            new Conversation(true, dialogStage.stage1, "Better not to touch it while its owner is here."),
            new Conversation(false, dialogStage.stage2, "There is nobody else here.")
        });
        dialogManager.setSpecialConversation(new Conversation(false, dialogStage.stage2, "There is nobodyelse here. Check the bookshelf?"));
    }

    protected override void SpecialEvent() {
        GameUIManager.instance.clearDialogBox();
        portalBookshelfUI.GetComponent<PortalBookshelfUI>().AccessBookshelf();
    }

    //---------------------------Subclass for dialogmanager of PortalBookshelf-------------------
    private class PortalBookshelfDialogManager : DialogManager {
        protected override bool isSpecialConversation() {
            return GameManager.instance.stageManager.currentStage == GameManager.StageManager.Stage.stage2;
        }
    }
}
