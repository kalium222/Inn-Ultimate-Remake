using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Dialoues;

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

    override protected bool IsSpecialConversation() {
        return GameManager.Instance.gameStageManager.CurrentStage == GameStageManager.Stage.Exploring;
    }

    protected override void DoSpecialEvent() {
        portalBookshelfController.GetComponent<PortalBookshelfUI>().AccessBookshelf();
    }
}
